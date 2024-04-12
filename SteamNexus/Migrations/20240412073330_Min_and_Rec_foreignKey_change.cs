using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteamNexus.Migrations
{
    /// <inheritdoc />
    public partial class Min_and_Rec_foreignKey_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_MinimumRequirements",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_RecommendedRequirements",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_MinReqId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_RecReqId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "MinReqId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "RecReqId",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "RecommendedRequirements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "MinimumRequirements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedRequirements_GameId",
                table: "RecommendedRequirements",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_MinimumRequirements_GameId",
                table: "MinimumRequirements",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_MinimumRequirements_Games",
                table: "MinimumRequirements",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendedRequirements_Games",
                table: "RecommendedRequirements",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MinimumRequirements_Games",
                table: "MinimumRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendedRequirements_Games",
                table: "RecommendedRequirements");

            migrationBuilder.DropIndex(
                name: "IX_RecommendedRequirements_GameId",
                table: "RecommendedRequirements");

            migrationBuilder.DropIndex(
                name: "IX_MinimumRequirements_GameId",
                table: "MinimumRequirements");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "RecommendedRequirements");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "MinimumRequirements");

            migrationBuilder.AddColumn<int>(
                name: "MinReqId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecReqId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_MinReqId",
                table: "Games",
                column: "MinReqId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_RecReqId",
                table: "Games",
                column: "RecReqId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_MinimumRequirements",
                table: "Games",
                column: "MinReqId",
                principalTable: "MinimumRequirements",
                principalColumn: "MinReqId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_RecommendedRequirements",
                table: "Games",
                column: "RecReqId",
                principalTable: "RecommendedRequirements",
                principalColumn: "RecReqId");
        }
    }
}
