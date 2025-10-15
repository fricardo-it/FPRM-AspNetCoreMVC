using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPRMAspNetCoreMVC.Migrations
{
    /// <inheritdoc />
    public partial class ChatReplies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MessageId",
                table: "Message",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_MessageId",
                table: "Message",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Message_MessageId",
                table: "Message",
                column: "MessageId",
                principalTable: "Message",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Message_MessageId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_MessageId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "Message");
        }
    }
}
