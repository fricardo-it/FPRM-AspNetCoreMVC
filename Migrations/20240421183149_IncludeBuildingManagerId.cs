using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPRMAspNetCoreMVC.Migrations
{
    /// <inheritdoc />
    public partial class IncludeBuildingManagerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ManagerId",
                table: "Building",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Building_ManagerId",
                table: "Building",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Building_User_ManagerId",
                table: "Building",
                column: "ManagerId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Building_User_ManagerId",
                table: "Building");

            migrationBuilder.DropIndex(
                name: "IX_Building_ManagerId",
                table: "Building");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Building");
        }
    }
}
