using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteamNexus_Server.Migrations
{
    /// <inheritdoc />
    public partial class Add_Ori : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OriCpu",
                table: "RecommendedRequirements",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriGpu",
                table: "RecommendedRequirements",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriRam",
                table: "RecommendedRequirements",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriCpu",
                table: "MinimumRequirements",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriGpu",
                table: "MinimumRequirements",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriRam",
                table: "MinimumRequirements",
                type: "nvarchar(300)",
                maxLength: 300,
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
