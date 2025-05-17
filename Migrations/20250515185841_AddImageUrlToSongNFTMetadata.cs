using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mu_marketplaceV0.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToSongNFTMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "SongNFTMetadata",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_url",
                table: "SongNFTMetadata");
        }
    }
}
