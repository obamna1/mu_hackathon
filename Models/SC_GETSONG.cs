using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mu_marketplaceV0.Models
{
    public class SC_GETSONG
    {
        [Key]
        public Guid uuid { get; set; }
        public string name { get; set; }
        public string credit_name { get; set; }
        public string isrc_value { get; set; }
        public string isrc_country_code { get; set; }
        public string isrc_country_name { get; set; }
        public DateTime? release_date { get; set; }
        public string copyright { get; set; }
        public string app_url { get; set; }
        public int? duration { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("explicit")]
        public bool Explicit { get; set; }
        public string language_code { get; set; }
        public string distributor { get; set; }
        // New: Soundcharts API image URL
public string? ImageUrl { get; set; }

// Soundcharts mapping columns
public string? type { get; set; }
[Column(TypeName = "nvarchar(max)")] public string? artists_json { get; set; }
[Column(TypeName = "nvarchar(max)")] public string? genres_json { get; set; }
[Column(TypeName = "nvarchar(max)")] public string? composers_json { get; set; }
[Column(TypeName = "nvarchar(max)")] public string? producers_json { get; set; }
[Column(TypeName = "nvarchar(max)")] public string? labels_json { get; set; }
[Column(TypeName = "nvarchar(max)")] public string? errors_json { get; set; }

public double? audio_acousticness { get; set; }
public double? audio_danceability { get; set; }
public double? audio_energy { get; set; }
public double? audio_instrumentalness { get; set; }
public int?    audio_key { get; set; }
public double? audio_liveness { get; set; }
public double? audio_loudness { get; set; }
public int?    audio_mode { get; set; }
public double? audio_speechiness { get; set; }
public double? audio_tempo { get; set; }
public int?    audio_time_signature { get; set; }
public double? audio_valence { get; set; }

public DateTime? last_synced { get; set; }
    }
}
