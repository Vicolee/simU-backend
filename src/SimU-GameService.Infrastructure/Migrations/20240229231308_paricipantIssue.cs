using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimU_GameService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class paricipantIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Participants",
                table: "Conversations");

            migrationBuilder.AddColumn<Guid>(
                name: "ParticipantA",
                table: "Conversations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ParticipantB",
                table: "Conversations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParticipantA",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "ParticipantB",
                table: "Conversations");

            migrationBuilder.AddColumn<string>(
                name: "Participants",
                table: "Conversations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
