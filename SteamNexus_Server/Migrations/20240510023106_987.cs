using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteamNexus_Server.Migrations
{
    /// <inheritdoc />
    public partial class _987 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductInformationId",
                table: "ProductMenus",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductMenus_ProductInformationId",
                table: "ProductMenus",
                column: "ProductInformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMenus_ProductInformations",
                table: "ProductMenus",
                column: "ProductInformationId",
                principalTable: "ProductInformations",
                principalColumn: "ProductInformationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMenus_ProductInformations",
                table: "ProductMenus");

            migrationBuilder.DropIndex(
                name: "IX_ProductMenus_ProductInformationId",
                table: "ProductMenus");

            migrationBuilder.DropColumn(
                name: "ProductInformationId",
                table: "ProductMenus");
        }
    }
}
