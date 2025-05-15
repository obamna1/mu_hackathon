using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using mu_marketplaceV0.Models;
using mu_marketplaceV0.ViewModels;
using mu_marketplaceV0.Services;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace mu_marketplaceV0.Controllers
{
    [Route("Mint")]
    public class MintController : Controller
    {
        private readonly SongMetaDbContext _context;
        private readonly IHttpClientFactory _httpFactory;

        public MintController(SongMetaDbContext context, IHttpClientFactory httpFactory)
        {
            _context = context;
            _httpFactory = httpFactory;
        }

        // POST /Mint
        [HttpGet("")]
        [HttpPost("")]
        public async Task<IActionResult> Index(int id)
        {
            var song = await _context.SongNFTMetadata.FindAsync(id);
            if (song == null) return NotFound("Song not found");

            // Use the image_url from the database (external link)
            // No SVG generation

            // Build JSON metadata with only the attributes array
            var meta = new
            {
                attributes = new[]
                {
                    new { trait_type = "Artist", value = song.Artist },
                    new { trait_type = "ISRC", value = song.Isrc ?? "SONG" }
                }
            };

            var metaJson = JsonSerializer.Serialize(meta);
            var tempPath = Path.Combine(Directory.GetCurrentDirectory(), "mint_input.json");
            await System.IO.File.WriteAllTextAsync(tempPath, metaJson);

            // Base64 encode the JSON (no gzip)
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(metaJson);
            var b64 = Convert.ToBase64String(jsonBytes);
            var dataUri = "data:application/json;base64," + b64;

            // Check length against 200-byte limit
            if (System.Text.Encoding.UTF8.GetByteCount(dataUri) > 200)
            {
                return StatusCode(400, $"Metadata URI too long: {System.Text.Encoding.UTF8.GetByteCount(dataUri)} bytes\n\n{dataUri}");
            }

            // Prepare the object for the mint script
            var mintInput = new
            {
                uri = dataUri,
                image_url = song.ImageUrl,
                title = song.Title,
                isrc = song.Isrc,
                artist = song.Artist
            };

            var mintInputJson = JsonSerializer.Serialize(mintInput);
            var mintInputPath = Path.Combine(Directory.GetCurrentDirectory(), "mint_input.json");
            await System.IO.File.WriteAllTextAsync(mintInputPath, mintInputJson);

            var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "mint_js", "mint.js");
            var nodeDir = Path.Combine(Directory.GetCurrentDirectory(), "mint_js");
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "node",
                    Arguments = $"\"{scriptPath}\" \"{mintInputPath}\"",
                    WorkingDirectory = nodeDir,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();
            process.WaitForExit();

            var combinedLog = $"Mint process finished\n\nSTDOUT:\n{output}\n\nSTDERR:\n{error}";

            if (process.ExitCode != 0)
            {
                return StatusCode(500, combinedLog);
            }

            // Parse successful mint details from STDOUT
            var mintMatch = Regex.Match(output, @"Mint:\s+([A-Za-z0-9]+)");
            var txMatch   = Regex.Match(output, @"Transaction:\s+([A-Za-z0-9]+)");

            var viewModel = new MintResultViewModel
            {
                MintAddress = mintMatch.Success ? mintMatch.Groups[1].Value : null,
                TransactionSignature = txMatch.Success ? txMatch.Groups[1].Value : null,
                ExplorerUrl = mintMatch.Success
                    ? $"https://explorer.solana.com/address/{mintMatch.Groups[1].Value}?cluster=testnet"
                    : null,
                ImageUrl = song.ImageUrl,
                RawOutput = output,
                RawError  = error
            };

            return View("Result", viewModel);

        }

    }
}
