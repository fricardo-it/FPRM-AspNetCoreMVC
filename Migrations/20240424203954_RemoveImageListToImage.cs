using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPRMAspNetCoreMVC.Migrations
{
    /// <inheritdoc />
    public partial class RemoveImageListToImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Apartment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageData",
                table: "Apartment",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
