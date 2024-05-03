using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteamNexus.Migrations
{
    /// <inheritdoc />
    public partial class AddOridata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OriCpu",
                table: "RecommendedRequirements",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriGpu",
                table: "RecommendedRequirements",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriRam",
                table: "RecommendedRequirements",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriCpu",
                table: "MinimumRequirements",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriGpu",
                table: "MinimumRequirements",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriRam",
                table: "MinimumRequirements",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriCpu",
                table: "RecommendedRequirements");

            migrationBuilder.DropColumn(
                name: "OriGpu",
                table: "RecommendedRequirements");

            migrationBuilder.DropColumn(
                name: "OriRam",
                table: "RecommendedRequirements");

            migrationBuilder.DropColumn(
                name: "OriCpu",
                table: "MinimumRequirements");

            migrationBuilder.DropColumn(
                name: "OriGpu",
                table: "MinimumRequirements");

            migrationBuilder.DropColumn(
                name: "OriRam",
                table: "MinimumRequirements");
        }
    }
}
