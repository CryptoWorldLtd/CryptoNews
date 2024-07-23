using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWorld.News.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyRegionToArticleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Region",
                table: "Articles");
        }
    }
}
