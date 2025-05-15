using Microsoft.EntityFrameworkCore;
using mu_marketplaceV0.Models;
using DotNetEnv; // Add this for .env support
using Microsoft.Extensions.DependencyInjection;
using mu_marketplaceV0.Services;

// Load .env into environment variables
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add environment variables from .env to configuration
builder.Configuration.AddEnvironmentVariables();

// Register EF Core DbContexts
builder.Services.AddDbContext<SoundchartsDbContext>(options =>
    options.UseSqlServer(builder.Configuration["SOUNDCHARTS_CONNECTION_STRING"]));

builder.Services.AddDbContext<SongMetaDbContext>(options =>
    options.UseSqlServer(builder.Configuration["NFT_CACHE_CONNECTION_STRING"]));

builder.Services.AddHttpClient();
builder.Services.AddSingleton<SolanaWalletService>();
// Add MVC services
builder.Services.AddControllersWithViews();

var app = builder.Build();

﻿// Provision and migrate databases
using (var scope = app.Services.CreateScope())
{
    // Ensure Soundcharts database exists (creates DB & tables if missing)
    var soundchartsDb = scope.ServiceProvider.GetRequiredService<SoundchartsDbContext>();
    soundchartsDb.Database.EnsureCreated();

    // Apply migrations to SongMeta database
    var metaDb = scope.ServiceProvider.GetRequiredService<SongMetaDbContext>();
    metaDb.Database.Migrate();

    // Test-seed one song so /Meta?id=1 works
    if (!metaDb.SongNFTMetadata.Any(x => x.Id == 1))
    {
        metaDb.SongNFTMetadata.Add(new SongNFTMetadata
        {
            Id = 1,
            Title = "BAD GUY",
            Isrc = "USUM71900764",
            Writer1 = "O'CONNELL BILLIE EILISH",
            Writer2 = "OCONNELL FINNEAS BAIRD",
            Publisher1 = "DRUP",
            Publisher2 = "LAST FRONTIER",
            AscapShare = 75,
            Artist = "Billie Eilish",
            ReleaseDate = DateTime.Parse("2019-03-28"),
            Copyright = "© 2019 Darkroom/Interscope Records",
            DurationSeconds = 194,
            Explicit = false,
            Language = "EN",
            Distributor = "Universal",
            OriginCountry = "United States"
        });
        metaDb.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
