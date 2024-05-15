using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAUthenticationImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "faceAuthenticationImage",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "faceAuthenticationImage",
                table: "Users");
        }
    }
}
