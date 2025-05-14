using Microsoft.EntityFrameworkCore;
using mu_marketplaceV0.Models;

public class SongMetaDbContext : DbContext
{
    public SongMetaDbContext(DbContextOptions<SongMetaDbContext> options)
        : base(options)
    {
    }

    public DbSet<SongNFTMetadata> SongNFTMetadata { get; set; }
}
