using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mu_marketplaceV0.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

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
            var jObj = (JObject)data.@object;

            var song = new Song
            {
                uuid = Guid.Parse((string)jObj["uuid"]),
                type = (string)data.type,
                name = (string)jObj["name"],
                isrc_value = (string?)jObj["isrc"]?["value"],
                isrc_country_code = (string?)jObj["isrc"]?["countryCode"],
                isrc_country_name = (string?)jObj["isrc"]?["countryName"],
                credit_name = (string?)jObj["creditName"],
                artist_uuid = jObj["artists"]?.First?["uuid"] != null 
                              ? Guid.Parse((string)jObj["artists"].First["uuid"]) 
                              : (Guid?)null,
                artist_slug = (string?)jObj["artists"]?.First?["slug"],
                artist_name = (string?)jObj["artists"]?.First?["name"],
                artist_app_url = (string?)jObj["artists"]?.First?["appUrl"],
                artist_image_url = (string?)jObj["artists"]?.First?["imageUrl"],
                release_date = jObj["releaseDate"] != null 
                               ? (DateTimeOffset?)DateTimeOffset.Parse((string)jObj["releaseDate"]) 
                               : null,
                copyright = (string?)jObj["copyright"],
                app_url = (string?)jObj["appUrl"],
                image_url = (string?)jObj["imageUrl"],
                duration = (int?)jObj["duration"],
                Explicit = (bool?)jObj["explicit"] ?? false,
                genres = jObj["genres"] != null 
                         ? string.Join(",", jObj["genres"].Select(g => $"{(string)g["root"]}:{(string)g["sub"][0]}")) 
                         : null,
                composers = jObj["composers"] != null 
                            ? string.Join(",", jObj["composers"].ToObject<List<string>>()) 
                            : null,
                producers = jObj["producers"] != null 
                            ? string.Join(",", jObj["producers"].ToObject<List<string>>()) 
                            : null,
                labels = jObj["labels"] != null 
                         ? string.Join(",", jObj["labels"].Select(l => $"{(string)l["name"]}:{(string)l["type"]}")) 
                         : null,
                audio_acousticness = (double?)jObj["audio"]?["acousticness"],
                audio_danceability = (double?)jObj["audio"]?["danceability"],
                audio_energy = (double?)jObj["audio"]?["energy"],
                audio_instrumentalness = (double?)jObj["audio"]?["instrumentalness"],
                audio_key = (int?)jObj["audio"]?["key"],
                audio_liveness = (double?)jObj["audio"]?["liveness"],
                audio_loudness = (double?)jObj["audio"]?["loudness"],
                audio_mode = (int?)jObj["audio"]?["mode"],
                audio_speechiness = (double?)jObj["audio"]?["speechiness"],
                audio_tempo = (double?)jObj["audio"]?["tempo"],
                audio_time_signature = (int?)jObj["audio"]?["timeSignature"],
                audio_valence = (double?)jObj["audio"]?["valence"],
                language_code = (string?)jObj["languageCode"],
                distributor = (string?)jObj["distributor"]
            };

            try
            {
                // Check if the song already exists
                var existingSong = await _context.Songs
                                          .FirstOrDefaultAsync(s => s.uuid == song.uuid);
                
                if (existingSong != null)
                {
                    // Keep the original primary key
                    song.song_id = existingSong.song_id;
                    
                    // Update the existing record with fresh values
                    _context.Entry(existingSong).CurrentValues.SetValues(song);
                    await _context.SaveChangesAsync();
                    return View("PingResult", existingSong);
                }
                
                // Add as a new record
                _context.Songs.Add(song);
                await _context.SaveChangesAsync();
                return View("PingResult", song);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error saving song metadata: {ex.Message}");
            }
        }
    }
}
