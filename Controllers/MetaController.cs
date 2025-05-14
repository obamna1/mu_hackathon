using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Encodings.Web;
using mu_marketplaceV0.Models;
using mu_marketplaceV0.ViewModels;
using System.Threading.Tasks;

namespace mu_marketplaceV0.Controllers
{
    [Route("Meta")]
    public class MetaController : Controller
    {
        private readonly SongMetaDbContext _context;

        public MetaController(SongMetaDbContext context)
        {
            _context = context;
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

            var vm = new MetaViewModel
            {
                Song = song,
                Json = JsonSerializer.Serialize(song, options)
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

            var imageUrl = $"https://en.wikipedia.org/wiki/Special:FilePath/{Uri.EscapeDataString(song.Title)}_cover.jpg";

            var metadata = new
            {
                name = song.Title,
                symbol = song.Isrc?.Substring(0, 4).ToUpper() ?? "SONG",
                description = $"A verified rights NFT for {song.Title} by {song.Artist}.",
                image = imageUrl,
                attributes = new List<object>
        {
            new { trait_type = "Artist", value = song.Artist },
            new { trait_type = "Release Date", value = song.ReleaseDate.ToString("yyyy-MM-dd") },
            new { trait_type = "Explicit", value = song.Explicit },
            new { trait_type = "Language", value = song.Language },
            new { trait_type = "Duration", value = song.DurationSeconds },
            new { trait_type = "Country", value = song.OriginCountry },
            new { trait_type = "Distributor", value = song.Distributor }
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
