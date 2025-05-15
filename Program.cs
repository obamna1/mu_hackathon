using Microsoft.EntityFrameworkCore;
using mu_marketplaceV0.Models;
using DotNetEnv; // Add this for .env support
using Microsoft.Extensions.DependencyInjection;

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
