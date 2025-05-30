﻿using Microsoft.EntityFrameworkCore;
using mu_marketplaceV0.Models;
using System.Collections.Generic;

namespace mu_marketplaceV0.Models
{
    public class SoundchartsDbContext : DbContext
    {
        public SoundchartsDbContext(DbContextOptions<SoundchartsDbContext> options)
            : base(options)
        {
        }

        public DbSet<SC_GETSONG> SC_GETSONG { get; set; }
        public DbSet<Song> Songs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Song>()
                .HasIndex(s => s.uuid)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
