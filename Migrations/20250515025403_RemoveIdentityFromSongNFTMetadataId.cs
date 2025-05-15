using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mu_marketplaceV0.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIdentityFromSongNFTMetadataId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SongNFTMetadata",
                table: "SongNFTMetadata");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SongNFTMetadata");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SongNFTMetadata",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SongNFTMetadata",
                table: "SongNFTMetadata",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SongNFTMetadata",
                table: "SongNFTMetadata");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SongNFTMetadata");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SongNFTMetadata",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SongNFTMetadata",
                table: "SongNFTMetadata",
                column: "Id");
        }
    }
}
