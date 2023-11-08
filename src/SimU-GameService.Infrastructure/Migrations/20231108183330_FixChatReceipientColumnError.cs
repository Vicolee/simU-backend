using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimU_GameService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixChatReceipientColumnError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SenderID",
                table: "Chats",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "ReceiverID",
                table: "Chats",
                newName: "RecipientId");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Chats",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Chats",
                newName: "SenderID");

            migrationBuilder.RenameColumn(
                name: "RecipientId",
                table: "Chats",
                newName: "ReceiverID");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Chats",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
