using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mu_marketplaceV0.Migrations.SoundchartsDb
{
    public partial class AddSoundchartsFullSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add new columns instead of recreating the table
            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "SC_GETSONG",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "artists_json",
                table: "SC_GETSONG",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "genres_json",
                table: "SC_GETSONG",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "composers_json",
                table: "SC_GETSONG",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "producers_json",
                table: "SC_GETSONG",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "labels_json",
                table: "SC_GETSONG",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "errors_json",
                table: "SC_GETSONG",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "audio_acousticness",
                table: "SC_GETSONG",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "audio_danceability",
                table: "SC_GETSONG",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "audio_energy",
                table: "SC_GETSONG",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "audio_instrumentalness",
                table: "SC_GETSONG",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "audio_key",
                table: "SC_GETSONG",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "audio_liveness",
                table: "SC_GETSONG",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "audio_loudness",
                table: "SC_GETSONG",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "audio_mode",
                table: "SC_GETSONG",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "audio_speechiness",
                table: "SC_GETSONG",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "audio_tempo",
                table: "SC_GETSONG",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "audio_time_signature",
                table: "SC_GETSONG",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "audio_valence",
                table: "SC_GETSONG",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove columns when rolling back
            migrationBuilder.DropColumn(
                name: "type",
                table: "SC_GETSONG");

            migrationBuilder.DropColumn(
                name: "artists_json",
                table: "SC_GETSONG");

            migrationBuilder.DropColumn(
                name: "genres_json",
                table: "SC_GETSONG");

            migrationBuilder.DropColumn(
                name: "composers_json",
                table: "SC_GETSONG");

            migrationBuilder.DropColumn(
                name: "producers_json",
                table: "SC_GETSONG");

            migrationBuilder.DropColumn(
                name: "labels_json",
                table: "SC_GETSONG");

            migrationBuilder.DropColumn(
                name: "errors_json",
                table: "SC_GETSONG");

            migrationBuilder.DropColumn(
                name: "audio_acousticness",
                table: "SC_GETSONG");

            migrationBuilder.DropColumn(
                name: "audio_danceability",
                table: "SC_GETSONG");

            migrationBuilder.DropColumn(
                name: "audio_energy",
                table: "SC_GETSONG");

            migrationBuilder.DropColumn(
                name: "audio_instrumentalness",
                table: "SC_GETSONG");

            migrationBuilder.DropColumn(
                name: "audio_key",
                table: "SC_GETSONG");

            migrationBuilder.DropColumn(
                name: "audio_liveness",
                table: "SC_GETSONG");

            migrationBuilder.DropColumn(
                name: "audio_loudness",
                table: "SC_GETSONG");

            migrationBuilder.DropColumn(
                name: "audio_mode",
                table: "SC_GETSONG");

            migrationBuilder.DropColumn(
                name: "audio_speechiness",
                table: "SC_GETSONG");

            migrationBuilder.DropColumn(
                name: "audio_tempo",
                table: "SC_GETSONG");

            migrationBuilder.DropColumn(
                name: "audio_time_signature",
                table: "SC_GETSONG");

            migrationBuilder.DropColumn(
                name: "audio_valence",
                table: "SC_GETSONG");
        }
    }
}
