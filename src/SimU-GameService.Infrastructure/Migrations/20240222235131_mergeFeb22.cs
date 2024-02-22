using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimU_GameService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mergeFeb22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpriteHeadshotURL",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SpriteURL",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "WorldCode",
                table: "Worlds",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentityId",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActiveWorldId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<List<int>>(
                name: "SpriteAnimations",
                table: "Users",
                type: "integer[]",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "QuestionNumber",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveWorldId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SpriteAnimations",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "QuestionNumber",
                table: "Questions");

            migrationBuilder.AlterColumn<string>(
                name: "WorldCode",
                table: "Worlds",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityId",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "SpriteHeadshotURL",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpriteURL",
                table: "Users",
                type: "text",
                nullable: true);
        }
    }
}
