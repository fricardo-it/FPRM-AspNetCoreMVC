using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPRMAspNetCoreMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddImageToApartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Apartment",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Apartment");
        }
    }
}
