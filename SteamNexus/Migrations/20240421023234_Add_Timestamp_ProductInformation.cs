using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteamNexus.Migrations
{
    /// <inheritdoc />
    public partial class Add_Timestamp_ProductInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "ProductInformations",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "ProductInformations");
        }
    }
}
