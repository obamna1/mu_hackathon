// Controllers/MetaController.cs
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Encodings.Web;
using mu_marketplaceV0.Models;
using System.Collections.Generic;
using mu_marketplaceV0.ViewModels;
using System.Threading.Tasks;
using mu_marketplaceV0.Services;

namespace mu_marketplaceV0.Controllers
{
    [Route("Meta")]
    public class MetaController : Controller
    {
        private readonly SongMetaDbContext _context;
        private readonly SolanaWalletService _walletService;

        public MetaController(SongMetaDbContext context, SolanaWalletService walletService)
        {
            _context = context;
            _walletService = walletService;
        }

        // GET /Meta?id=1
        [HttpGet("")]
        public async Task<IActionResult> Index(int id = 1)
        {
            var song = await _context.SongNFTMetadata.FindAsync(id);
            if (song == null)
                return NotFound();

            CleanMetadata(song);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var walletAddress = _walletService.GetPublicAddress();
            var tokensBySong = await _walletService.GetTokensBySongAsync();

            var vm = new MetaViewModel
            {
                Song = song,
                Json = JsonSerializer.Serialize(song, options),
                WalletAddress = walletAddress,
                TokensBySong = tokensBySong
            };

            ViewData["Title"] = "Metadata Test";
            return View(vm);
        }

        // GET /metadata/{id}.json
        [HttpGet("/metadata/{id}.json")]
        [Produces("application/json")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> MetadataFile(int id)
        {
            var song = await _context.SongNFTMetadata.FindAsync(id);
            if (song == null)
                return NotFound();

            CleanMetadata(song);

            var metadata = new
            {
                name = song.Title,
                symbol = song.Isrc?.Substring(0, 4).ToUpper() ?? "SONG",
                description = $"A verified rights NFT for {song.Title} by {song.Artist}.",
                image = "https://solana.com/src/img/branding/solanaLogoMark.svg",
                attributes = new List<object>
                {
                    new { trait_type = "Artist",       value = song.Artist },
                    new { trait_type = "Title",        value = song.Title },
                    new { trait_type = "Release Date", value = song.ReleaseDate.ToString("yyyy-MM-dd") },
                    new { trait_type = "ISRC",         value = song.Isrc }
                }
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            return new JsonResult(metadata, options);
        }

        private void CleanMetadata(SongNFTMetadata song)
        {
            if (!string.IsNullOrEmpty(song.Copyright))
                song.Copyright = song.Copyright.Replace("©", "").Trim();
        }
    }
}
