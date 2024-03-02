using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SimU_GameService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SplitQuestionResponsesTableFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgentQuestionResponses_Agents_Id",
                table: "AgentQuestionResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserQuestionResponses_Users_Id",
                table: "UserQuestionResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserQuestionResponses",
                table: "UserQuestionResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AgentQuestionResponses",
                table: "AgentQuestionResponses");

            migrationBuilder.DropColumn(
                name: "Id1",
                table: "UserQuestionResponses");

            migrationBuilder.DropColumn(
                name: "Id1",
                table: "AgentQuestionResponses");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserQuestionResponses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AgentId",
                table: "AgentQuestionResponses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserQuestionResponses",
                table: "UserQuestionResponses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AgentQuestionResponses",
                table: "AgentQuestionResponses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestionResponses_UserId",
                table: "UserQuestionResponses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentQuestionResponses_AgentId",
                table: "AgentQuestionResponses",
                column: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgentQuestionResponses_Agents_AgentId",
                table: "AgentQuestionResponses",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuestionResponses_Users_UserId",
                table: "UserQuestionResponses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgentQuestionResponses_Agents_AgentId",
                table: "AgentQuestionResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserQuestionResponses_Users_UserId",
                table: "UserQuestionResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserQuestionResponses",
                table: "UserQuestionResponses");

            migrationBuilder.DropIndex(
                name: "IX_UserQuestionResponses_UserId",
                table: "UserQuestionResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AgentQuestionResponses",
                table: "AgentQuestionResponses");

            migrationBuilder.DropIndex(
                name: "IX_AgentQuestionResponses_AgentId",
                table: "AgentQuestionResponses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserQuestionResponses");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "AgentQuestionResponses");

            migrationBuilder.AddColumn<int>(
                name: "Id1",
                table: "UserQuestionResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id1",
                table: "AgentQuestionResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserQuestionResponses",
                table: "UserQuestionResponses",
                columns: new[] { "Id", "Id1" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AgentQuestionResponses",
                table: "AgentQuestionResponses",
                columns: new[] { "Id", "Id1" });

            migrationBuilder.AddForeignKey(
                name: "FK_AgentQuestionResponses_Agents_Id",
                table: "AgentQuestionResponses",
                column: "Id",
                principalTable: "Agents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuestionResponses_Users_Id",
                table: "UserQuestionResponses",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
