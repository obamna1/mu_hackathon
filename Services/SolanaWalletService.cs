// Services/SolanaWalletService.cs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DotNetEnv;
using Microsoft.Extensions.Logging;
using mu_marketplaceV0.Models;
using Org.BouncyCastle.Crypto.Parameters;
using Solnet.Wallet.Utilities;

namespace mu_marketplaceV0.Services
{
    public class SolanaWalletService
    {
        private readonly string _owner;
        private readonly HttpClient _client;
        private readonly ILogger<SolanaWalletService> _logger;
        private const string TOKEN_PROGRAM_ID = "TokenkegQfeZyiNwAJbNbGKPFXCWuBvf9Ss623VQ5DA";

        public SolanaWalletService(IHttpClientFactory httpFactory, ILogger<SolanaWalletService> logger)
        {
            Env.Load();
            var secretKeyBytes = Encoders.Base58.DecodeData(Environment.GetEnvironmentVariable("SOLANA_SECRET_KEY"));
            var seed = new byte[32];
            Array.Copy(secretKeyBytes, 0, seed, 0, 32);
            var privParam = new Ed25519PrivateKeyParameters(seed, 0);
            var pubParam = privParam.GeneratePublicKey();
            var pubKeyBytes = pubParam.GetEncoded();
            _owner = Encoders.Base58.EncodeData(pubKeyBytes);

            _client = httpFactory.CreateClient();
            _logger = logger;
        }

        public string GetPublicAddress() => _owner;

        public async Task<Dictionary<int, List<TokenInfo>>> GetTokensBySongAsync()
        {
            var result = new Dictionary<int, List<TokenInfo>>();
            var rpcUrl = Environment.GetEnvironmentVariable("SOLANA_URL");
            var payload = new
            {
                jsonrpc = "2.0",
                id = 1,
                method = "getTokenAccountsByOwner",
                @params = new object[]
                {
                    _owner,
                    new { programId = TOKEN_PROGRAM_ID },
                    new { encoding = "jsonParsed", commitment = "confirmed" }
                }
            };

            try
            {
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(rpcUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("RPC HTTP call failed: {0}", response.StatusCode);
                    return result;
                }

                var body = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(body);
                var arr = doc.RootElement.GetProperty("result").GetProperty("value");
                foreach (var entry in arr.EnumerateArray())
                {
                    var info = entry.GetProperty("account").GetProperty("data").GetProperty("parsed").GetProperty("info");
                    var amount = info.GetProperty("tokenAmount").GetProperty("uiAmount").GetDecimal();
                    if (amount <= 0) continue;
                    var mint = info.GetProperty("mint").GetString();
                    var explorerUrl = $"https://explorer.solana.com/address/{mint}?cluster=testnet";
                    var tokenInfo = new TokenInfo(mint, amount, explorerUrl);

                    if (!result.ContainsKey(0))
                        result[0] = new List<TokenInfo>();
                    result[0].Add(tokenInfo);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tokens");
            }

            return result;
        }
    }
}
