using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPRMAspNetCoreMVC.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMsgChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Message_MessageId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_SenderUserId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_MessageId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ParentMessageId",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "SenderUserId",
                table: "Message",
                newName: "SenderMsgId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_SenderUserId",
                table: "Message",
                newName: "IX_Message_SenderMsgId");

            migrationBuilder.CreateTable(
                name: "Reply",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderReplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParentMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reply_Message_ParentMessageId",
                        column: x => x.ParentMessageId,
                        principalTable: "Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reply_ParentMessageId",
                table: "Reply",
                column: "ParentMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_SenderMsgId",
                table: "Message",
                column: "SenderMsgId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_SenderMsgId",
                table: "Message");

            migrationBuilder.DropTable(
                name: "Reply");

            migrationBuilder.RenameColumn(
                name: "SenderMsgId",
                table: "Message",
                newName: "SenderUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_SenderMsgId",
                table: "Message",
                newName: "IX_Message_SenderUserId");

            migrationBuilder.AddColumn<Guid>(
                name: "MessageId",
                table: "Message",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentMessageId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_SenderUserId",
                table: "Message",
                column: "SenderUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
