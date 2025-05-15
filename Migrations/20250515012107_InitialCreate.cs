using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mu_marketplaceV0.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SongNFTMetadata",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isrc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    writer_1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    writer_2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    publisher_1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    publisher_2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ascap_share = table.Column<int>(type: "int", nullable: false),
                    artist = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    release_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    copyright = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    duration_seconds = table.Column<int>(type: "int", nullable: false),
                    @explicit = table.Column<bool>(name: "explicit", type: "bit", nullable: false),
                    language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    distributor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    origin_country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongNFTMetadata", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SongNFTMetadata");
        }
    }
}
