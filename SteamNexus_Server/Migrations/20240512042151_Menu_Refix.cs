using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteamNexus_Server.Migrations
{
    /// <inheritdoc />
    public partial class Menu_Refix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Menus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TotalPrice",
                table: "Menus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "Menus",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Menus");
        }
    }
}
