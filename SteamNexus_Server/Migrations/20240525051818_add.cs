using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteamNexus_Server.Migrations
{
    /// <inheritdoc />
    public partial class add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "GameTrackings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "GameTrackings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
