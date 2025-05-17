using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace mu_marketplaceV0.Models
{
    [Table("SongNFTMetadata")]
    public class SongNFTMetadata
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("isrc")]
        public string Isrc { get; set; }

        [Column("writer_1")]
        public string Writer1 { get; set; }

        [Column("writer_2")]
        public string Writer2 { get; set; }

        [Column("publisher_1")]
        public string Publisher1 { get; set; }

        [Column("publisher_2")]
        public string Publisher2 { get; set; }

        [Column("ascap_share")]
        public int AscapShare { get; set; }

        [Column("artist")]
        public string Artist { get; set; }

        [Column("release_date")]
        public DateTime ReleaseDate { get; set; }

        [Column("copyright")]
        public string Copyright { get; set; }

        [Column("duration_seconds")]
        public int DurationSeconds { get; set; }

        [Column("explicit")]
        public bool Explicit { get; set; }

        [Column("language")]
        public string Language { get; set; }

        [Column("distributor")]
        public string Distributor { get; set; }

        [Column("origin_country")]
        public string OriginCountry { get; set; }

        [Column("image_url")]
        public string? ImageUrl { get; set; }
    }
}
