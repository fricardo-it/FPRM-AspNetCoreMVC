using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPRMAspNetCoreMVC.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMessageToReplyList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Replies",
                table: "Message");

            migrationBuilder.CreateTable(
                name: "Reply",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderReplyIdId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reply_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reply_User_SenderReplyIdId",
                        column: x => x.SenderReplyIdId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reply_MessageId",
                table: "Reply",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Reply_SenderReplyIdId",
                table: "Reply",
                column: "SenderReplyIdId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reply");

            migrationBuilder.AddColumn<string>(
                name: "Replies",
                table: "Message",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
