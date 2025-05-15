using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mu_marketplaceV0.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace mu_marketplaceV0.Controllers
{
    [Route("Soundcharts")]
    public class SoundchartsController : Controller
    {
        private readonly SoundchartsDbContext _context;
        private readonly IConfiguration _config;

        public SoundchartsController(SoundchartsDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return View();
        }

        [HttpPost("Ping")]
        public async Task<IActionResult> Ping(string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return BadRequest("UUID is required.");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-app-id", _config["SOUNDCHARTS_APP_ID"]);
            client.DefaultRequestHeaders.Add("x-api-key", _config["SOUNDCHARTS_APP_KEY"]);

            var baseUrl = _config["SOUNDCHARTS_BASE_URL"]?.TrimEnd('/');
            if (string.IsNullOrWhiteSpace(baseUrl))
                return StatusCode(500, "SOUNDCHARTS_BASE_URL is not configured properly.");

            var fullUrl = $"{baseUrl}/api/v2.25/song/{uuid}";
            Console.WriteLine($"[DEBUG] Calling Soundcharts URL: {fullUrl}");

            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync(fullUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"HTTP request failed: {ex.Message}");
            }

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to fetch song metadata.");

            var json = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(json);

            var song = new SC_GETSONG
            {
                uuid = Guid.Parse((string)data.@object.uuid),
                name = (string)data.@object.name,
                credit_name = (string)data.@object.creditName,
                isrc_value = (string)data.@object.isrc.value,
                isrc_country_code = (string)data.@object.isrc.countryCode,
                isrc_country_name = (string)data.@object.isrc.countryName,
                release_date = (DateTime?)data.@object.releaseDate,
                copyright = (string)data.@object.copyright,
                app_url = (string)data.@object.appUrl,
                duration = (int?)data.@object.duration,
                Explicit = (bool?)data.@object.@explicit ?? false,
                language_code = (string)data.@object.languageCode,
                distributor = (string)data.@object.distributor,
                last_synced = DateTime.UtcNow
            };

            _context.SC_GETSONG.Add(song);
            await _context.SaveChangesAsync();

            return Ok("Song metadata inserted.");
        }
    }
}
