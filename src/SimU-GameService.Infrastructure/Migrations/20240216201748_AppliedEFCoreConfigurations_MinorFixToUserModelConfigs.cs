using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimU_GameService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AppliedEFCoreConfigurations_MinorFixToUserModelConfigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WorldsJoined",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(List<Guid>),
                oldType: "uuid[]");

            migrationBuilder.AlterColumn<string>(
                name: "WorldsCreated",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(List<Guid>),
                oldType: "uuid[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<Guid>>(
                name: "WorldsJoined",
                table: "Users",
                type: "uuid[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<List<Guid>>(
                name: "WorldsCreated",
                table: "Users",
                type: "uuid[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
