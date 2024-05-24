using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteamNexus_Server.Migrations
{
    /// <inheritdoc />
    public partial class GameTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameTrackings",
                columns: table => new
                {
                    GameTrackingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TrackingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTrackings", x => x.GameTrackingId);
                    table.ForeignKey(
                        name: "FK_GameTrackings_Games",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId");
                    table.ForeignKey(
                        name: "FK_GameTrackings_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameTrackings_GameId",
                table: "GameTrackings",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameTrackings_UserId",
                table: "GameTrackings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameTrackings");
        }
    }
}
