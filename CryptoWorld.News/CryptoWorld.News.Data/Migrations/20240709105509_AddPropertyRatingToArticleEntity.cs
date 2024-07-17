using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWorld.News.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyRatingToArticleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Articles");
        }
    }
}
