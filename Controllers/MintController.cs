using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using mu_marketplaceV0.Models;
using System.Diagnostics;
using System.Text.Json;

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

            var metadata = new
            {
                title = song.Title,
                isrc = song.Isrc,
                writers = new[] { song.Writer1, song.Writer2 },
                publishers = new[] { song.Publisher1, song.Publisher2 },
                ascap_share = song.AscapShare,
                artist = song.Artist,
                release_date = song.ReleaseDate.ToString("yyyy-MM-dd"),
                copyright = song.Copyright,
                duration_seconds = song.DurationSeconds,
                explicit_ = song.Explicit,
                language = song.Language,
                distributor = song.Distributor,
                origin_country = song.OriginCountry,

                uri = $"{Request.Scheme}://{Request.Host}/metadata/{id}.json"
            };

            var json = JsonSerializer.Serialize(metadata, new JsonSerializerOptions { WriteIndented = true });
            var tempPath = Path.Combine(Directory.GetCurrentDirectory(), "mint_temp.json");
            await System.IO.File.WriteAllTextAsync(tempPath, json);

            var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "mint_js", "mint.js");
            // move out of wwwroot!
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "node",
                    Arguments = $"\"{scriptPath}\" \"{tempPath}\"",
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

            return Content(combinedLog);

        }

    }
}
