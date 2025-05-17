// ViewModels/MetaViewModel.cs
using mu_marketplaceV0.Models;
using System.Collections.Generic;

namespace mu_marketplaceV0.ViewModels
{
    public class MetaViewModel
    {
        public required SongNFTMetadata Song { get; init; }
        public required string Json { get; init; }

        // Solana wallet address
        public required string WalletAddress { get; init; }

        // NFTs grouped by Song ID
        public required Dictionary<int, List<TokenInfo>> TokensBySong { get; init; }
    }
}
