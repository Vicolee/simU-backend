using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SimU_GameService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AppliedEFCoreConfigurations_MinorFixToModelConfigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Users_UserId",
                table: "Friend");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Location_LocationId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "QuestionResponse");

            migrationBuilder.DropIndex(
                name: "IX_Users_LocationId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "Summary");

            migrationBuilder.RenameColumn(
                name: "IsAgent",
                table: "Users",
                newName: "IsOnline");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Users",
                newName: "SpriteURL");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Groups",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Friend",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AgentId",
                table: "Agents",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Agents",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Agents",
                newName: "Summary");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Agents",
                newName: "SpriteURL");

            migrationBuilder.AddColumn<string>(
                name: "SpriteHeadshotURL",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<List<Guid>>(
                name: "WorldsCreated",
                table: "Users",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.AddColumn<List<Guid>>(
                name: "WorldsJoined",
                table: "Users",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "X_coord",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Y_coord",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ConversationId",
                table: "Chats",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "WasSenderOnline",
                table: "Chats",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "HatchTime",
                table: "Agents",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SpriteHeadshotURL",
                table: "Agents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Agents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "X_coord",
                table: "Agents",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Y_coord",
                table: "Agents",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Agents_QuestionResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Id1 = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResponderId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents_QuestionResponses", x => new { x.Id, x.Id1 });
                    table.ForeignKey(
                        name: "FK_Agents_QuestionResponses_Agents_Id",
                        column: x => x.Id,
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastMessageSentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Participants = table.Column<List<Guid>>(type: "uuid[]", nullable: false),
                    IsGroupChat = table.Column<bool>(type: "boolean", nullable: false),
                    IsConversationOver = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    QuestionType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users_QuestionResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Id1 = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResponderId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users_QuestionResponses", x => new { x.Id, x.Id1 });
                    table.ForeignKey(
                        name: "FK_Users_QuestionResponses_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Worlds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WorldUsers = table.Column<List<Guid>>(type: "uuid[]", nullable: false),
                    WorldAgents = table.Column<List<Guid>>(type: "uuid[]", nullable: false),
                    ThumbnailURL = table.Column<string>(type: "text", nullable: true),
                    WorldCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worlds", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Users_Id",
                table: "Friend",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Users_Id",
                table: "Friend");

            migrationBuilder.DropTable(
                name: "Agents_QuestionResponses");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Users_QuestionResponses");

            migrationBuilder.DropTable(
                name: "Worlds");

            migrationBuilder.DropColumn(
                name: "SpriteHeadshotURL",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WorldsCreated",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WorldsJoined",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "X_coord",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Y_coord",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "WasSenderOnline",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "HatchTime",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "SpriteHeadshotURL",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "X_coord",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "Y_coord",
                table: "Agents");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "SpriteURL",
                table: "Users",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "IsOnline",
                table: "Users",
                newName: "IsAgent");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Groups",
                newName: "GroupId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Friend",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Agents",
                newName: "AgentId");

            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "Agents",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "SpriteURL",
                table: "Agents",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Agents",
                newName: "UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    X = table.Column<int>(type: "integer", nullable: false),
                    Y = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "QuestionResponse",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Question = table.Column<string>(type: "text", nullable: false),
                    Response = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionResponse", x => new { x.UserId, x.Id });
                    table.ForeignKey(
                        name: "FK_QuestionResponse_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_LocationId",
                table: "Users",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Users_UserId",
                table: "Friend",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Location_LocationId",
                table: "Users",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "LocationId");
        }
    }
}
