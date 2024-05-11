using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteamNexus_Server.Migrations
{
    /// <inheritdoc />
    public partial class Menu_finsih : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductMenus");

            migrationBuilder.DropColumn(
                name: "MenuDetailName",
                table: "MenuDetails");

            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "MenuDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductInformationId",
                table: "MenuDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    MenuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.MenuId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuDetails_MenuId",
                table: "MenuDetails",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuDetails_ProductInformationId",
                table: "MenuDetails",
                column: "ProductInformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuDetails_Menus",
                table: "MenuDetails",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuDetails_ProductInformations",
                table: "MenuDetails",
                column: "ProductInformationId",
                principalTable: "ProductInformations",
                principalColumn: "ProductInformationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuDetails_Menus",
                table: "MenuDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuDetails_ProductInformations",
                table: "MenuDetails");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_MenuDetails_MenuId",
                table: "MenuDetails");

            migrationBuilder.DropIndex(
                name: "IX_MenuDetails_ProductInformationId",
                table: "MenuDetails");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "MenuDetails");

            migrationBuilder.DropColumn(
                name: "ProductInformationId",
                table: "MenuDetails");

            migrationBuilder.AddColumn<string>(
                name: "MenuDetailName",
                table: "MenuDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProductMenus",
                columns: table => new
                {
                    ProductMenuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    ProductInformationId = table.Column<int>(type: "int", nullable: true),
                    MenuName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMenus", x => x.ProductMenuId);
                    table.ForeignKey(
                        name: "FK_ProductMenus_ProductInformations",
                        column: x => x.ProductInformationId,
                        principalTable: "ProductInformations",
                        principalColumn: "ProductInformationId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductMenus_ProductInformationId",
                table: "ProductMenus",
                column: "ProductInformationId");
        }
    }
}
