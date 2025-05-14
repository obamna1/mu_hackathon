using System.ComponentModel.DataAnnotations;

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
        public DateTime? last_synced { get; set; }
    }
}
