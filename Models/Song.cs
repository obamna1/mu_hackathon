using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mu_marketplaceV0.Models
{
    [Table("songs")]
    public class Song
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int song_id { get; set; }

        [Required]
        public Guid uuid { get; set; }

        [Required, MaxLength(50)]
        public string type { get; set; }

        [Required, MaxLength(255)]
        public string name { get; set; }

        [MaxLength(12)]
        public string? isrc_value { get; set; }

        [MaxLength(2)]
        public string? isrc_country_code { get; set; }

        [MaxLength(100)]
        public string? isrc_country_name { get; set; }

        [MaxLength(255)]
        public string? credit_name { get; set; }

        public Guid? artist_uuid { get; set; }

        [MaxLength(255)]
        public string? artist_slug { get; set; }

        [MaxLength(255)]
        public string? artist_name { get; set; }

        public string? artist_app_url { get; set; }

        public string? artist_image_url { get; set; }

        public DateTimeOffset? release_date { get; set; }

        public string? copyright { get; set; }

        public string? app_url { get; set; }

        public string? image_url { get; set; }

        public int? duration { get; set; }

        [Column("explicit")]
        public bool? Explicit { get; set; }

        public string? genres { get; set; }

        public string? composers { get; set; }

        public string? producers { get; set; }

        public string? labels { get; set; }

        public double? audio_acousticness { get; set; }

        public double? audio_danceability { get; set; }

        public double? audio_energy { get; set; }

        public double? audio_instrumentalness { get; set; }

        public int? audio_key { get; set; }

        public double? audio_liveness { get; set; }

        public double? audio_loudness { get; set; }

        public int? audio_mode { get; set; }

        public double? audio_speechiness { get; set; }

        public double? audio_tempo { get; set; }

        public int? audio_time_signature { get; set; }

        public double? audio_valence { get; set; }

        [MaxLength(2)]
        public string? language_code { get; set; }

        [MaxLength(255)]
        public string? distributor { get; set; }
    }
}
