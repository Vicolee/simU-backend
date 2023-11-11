using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimU_GameService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Location_LastKnownLocationLocationId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "LastKnownLocationLocationId",
                table: "Users",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_LastKnownLocationLocationId",
                table: "Users",
                newName: "IX_Users_LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Location_LocationId",
                table: "Users",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Location_LocationId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Users",
                newName: "LastKnownLocationLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_LocationId",
                table: "Users",
                newName: "IX_Users_LastKnownLocationLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Location_LastKnownLocationLocationId",
                table: "Users",
                column: "LastKnownLocationLocationId",
                principalTable: "Location",
                principalColumn: "LocationId");
        }
    }
}
