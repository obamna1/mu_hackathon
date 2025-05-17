using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mu_marketplaceV0.Migrations.SoundchartsDb
{
    /// <inheritdoc />
    public partial class CreateSongsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "songs",
                columns: table => new
                {
                    song_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    isrc_value = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    isrc_country_code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    isrc_country_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    credit_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    artist_uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    artist_slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    artist_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    artist_app_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    artist_image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    release_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    copyright = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    app_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    duration = table.Column<int>(type: "int", nullable: true),
                    @explicit = table.Column<bool>(name: "explicit", type: "bit", nullable: true),
                    genres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    composers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    producers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    labels = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    audio_acousticness = table.Column<double>(type: "float", nullable: true),
                    audio_danceability = table.Column<double>(type: "float", nullable: true),
                    audio_energy = table.Column<double>(type: "float", nullable: true),
                    audio_instrumentalness = table.Column<double>(type: "float", nullable: true),
                    audio_key = table.Column<int>(type: "int", nullable: true),
                    audio_liveness = table.Column<double>(type: "float", nullable: true),
                    audio_loudness = table.Column<double>(type: "float", nullable: true),
                    audio_mode = table.Column<int>(type: "int", nullable: true),
                    audio_speechiness = table.Column<double>(type: "float", nullable: true),
                    audio_tempo = table.Column<double>(type: "float", nullable: true),
                    audio_time_signature = table.Column<int>(type: "int", nullable: true),
                    audio_valence = table.Column<double>(type: "float", nullable: true),
                    language_code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    distributor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_songs", x => x.song_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_songs_uuid",
                table: "songs",
                column: "uuid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "songs");
        }
    }
}
