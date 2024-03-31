using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteamNexus.Migrations
{
    /// <inheritdoc />
    public partial class HardwareVer1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCpu_CPUs_Cpuid",
                table: "ProductCpu");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCpu_ProductInformation_ProductInformationId",
                table: "ProductCpu");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductGpu_GPUs_Gpuid",
                table: "ProductGpu");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductGpu_ProductInformation_ProductInformationId",
                table: "ProductGpu");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInformation_ProductCategory_ProductCategoryId",
                table: "ProductInformation");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRam_ProductInformation_ProductInformationId",
                table: "ProductRam");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductRam",
                table: "ProductRam");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductInformation",
                table: "ProductInformation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductGpu",
                table: "ProductGpu");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCpu",
                table: "ProductCpu");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory");

            migrationBuilder.RenameTable(
                name: "ProductRam",
                newName: "ProductRAMs");

            migrationBuilder.RenameTable(
                name: "ProductInformation",
                newName: "ProductInformations");

            migrationBuilder.RenameTable(
                name: "ProductGpu",
                newName: "ProductGPUs");

            migrationBuilder.RenameTable(
                name: "ProductCpu",
                newName: "ProductCPUs");

            migrationBuilder.RenameTable(
                name: "ProductCategory",
                newName: "ProductCategories");

            migrationBuilder.RenameColumn(
                name: "ProductRamid",
                table: "ProductRAMs",
                newName: "ProductRAMId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRam_ProductInformationId",
                table: "ProductRAMs",
                newName: "IX_ProductRAMs_ProductInformationId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductInformation_ProductCategoryId",
                table: "ProductInformations",
                newName: "IX_ProductInformations_ProductCategoryId");

            migrationBuilder.RenameColumn(
                name: "Gpuid",
                table: "ProductGPUs",
                newName: "GPUId");

            migrationBuilder.RenameColumn(
                name: "ProductGpuid",
                table: "ProductGPUs",
                newName: "ProductGPUId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductGpu_ProductInformationId",
                table: "ProductGPUs",
                newName: "IX_ProductGPUs_ProductInformationId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductGpu_Gpuid",
                table: "ProductGPUs",
                newName: "IX_ProductGPUs_GPUId");

            migrationBuilder.RenameColumn(
                name: "Cpuid",
                table: "ProductCPUs",
                newName: "CPUId");

            migrationBuilder.RenameColumn(
                name: "ProductCpuid",
                table: "ProductCPUs",
                newName: "ProductCPUId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCpu_ProductInformationId",
                table: "ProductCPUs",
                newName: "IX_ProductCPUs_ProductInformationId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCpu_Cpuid",
                table: "ProductCPUs",
                newName: "IX_ProductCPUs_CPUId");

            migrationBuilder.AlterColumn<int>(
                name: "ProductRAMId",
                table: "ProductRAMs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "10000, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Specification",
                table: "ProductInformations",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductInformations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductInformationId",
                table: "ProductInformations",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "10000, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "ProductGPUId",
                table: "ProductGPUs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "10000, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "ProductCPUId",
                table: "ProductCPUs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "10000, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductCategories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductCategoryId",
                table: "ProductCategories",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "10000, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductRAMs",
                table: "ProductRAMs",
                column: "ProductRAMId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductInformations",
                table: "ProductInformations",
                column: "ProductInformationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductGPUs",
                table: "ProductGPUs",
                column: "ProductGPUId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCPUs",
                table: "ProductCPUs",
                column: "ProductCPUId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategories",
                table: "ProductCategories",
                column: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCPUs_CPUs",
                table: "ProductCPUs",
                column: "CPUId",
                principalTable: "CPUs",
                principalColumn: "CPUId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCPUs_ProductInformations",
                table: "ProductCPUs",
                column: "ProductInformationId",
                principalTable: "ProductInformations",
                principalColumn: "ProductInformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGPUs_GPUs",
                table: "ProductGPUs",
                column: "GPUId",
                principalTable: "GPUs",
                principalColumn: "GPUId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGPUs_ProductInformations",
                table: "ProductGPUs",
                column: "ProductInformationId",
                principalTable: "ProductInformations",
                principalColumn: "ProductInformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInformations_ProductCategories",
                table: "ProductInformations",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRAMs_ProductInformations",
                table: "ProductRAMs",
                column: "ProductInformationId",
                principalTable: "ProductInformations",
                principalColumn: "ProductInformationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCPUs_CPUs",
                table: "ProductCPUs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCPUs_ProductInformations",
                table: "ProductCPUs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductGPUs_GPUs",
                table: "ProductGPUs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductGPUs_ProductInformations",
                table: "ProductGPUs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInformations_ProductCategories",
                table: "ProductInformations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRAMs_ProductInformations",
                table: "ProductRAMs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductRAMs",
                table: "ProductRAMs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductInformations",
                table: "ProductInformations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductGPUs",
                table: "ProductGPUs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCPUs",
                table: "ProductCPUs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategories",
                table: "ProductCategories");

            migrationBuilder.RenameTable(
                name: "ProductRAMs",
                newName: "ProductRam");

            migrationBuilder.RenameTable(
                name: "ProductInformations",
                newName: "ProductInformation");

            migrationBuilder.RenameTable(
                name: "ProductGPUs",
                newName: "ProductGpu");

            migrationBuilder.RenameTable(
                name: "ProductCPUs",
                newName: "ProductCpu");

            migrationBuilder.RenameTable(
                name: "ProductCategories",
                newName: "ProductCategory");

            migrationBuilder.RenameColumn(
                name: "ProductRAMId",
                table: "ProductRam",
                newName: "ProductRamid");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRAMs_ProductInformationId",
                table: "ProductRam",
                newName: "IX_ProductRam_ProductInformationId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductInformations_ProductCategoryId",
                table: "ProductInformation",
                newName: "IX_ProductInformation_ProductCategoryId");

            migrationBuilder.RenameColumn(
                name: "GPUId",
                table: "ProductGpu",
                newName: "Gpuid");

            migrationBuilder.RenameColumn(
                name: "ProductGPUId",
                table: "ProductGpu",
                newName: "ProductGpuid");

            migrationBuilder.RenameIndex(
                name: "IX_ProductGPUs_ProductInformationId",
                table: "ProductGpu",
                newName: "IX_ProductGpu_ProductInformationId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductGPUs_GPUId",
                table: "ProductGpu",
                newName: "IX_ProductGpu_Gpuid");

            migrationBuilder.RenameColumn(
                name: "CPUId",
                table: "ProductCpu",
                newName: "Cpuid");

            migrationBuilder.RenameColumn(
                name: "ProductCPUId",
                table: "ProductCpu",
                newName: "ProductCpuid");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCPUs_ProductInformationId",
                table: "ProductCpu",
                newName: "IX_ProductCpu_ProductInformationId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCPUs_CPUId",
                table: "ProductCpu",
                newName: "IX_ProductCpu_Cpuid");

            migrationBuilder.AlterColumn<int>(
                name: "ProductRamid",
                table: "ProductRam",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "10000, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Specification",
                table: "ProductInformation",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductInformation",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "ProductInformationId",
                table: "ProductInformation",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "10000, 1");

            migrationBuilder.AlterColumn<int>(
                name: "ProductGpuid",
                table: "ProductGpu",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "10000, 1");

            migrationBuilder.AlterColumn<int>(
                name: "ProductCpuid",
                table: "ProductCpu",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "10000, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductCategory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "ProductCategoryId",
                table: "ProductCategory",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "10000, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductRam",
                table: "ProductRam",
                column: "ProductRamid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductInformation",
                table: "ProductInformation",
                column: "ProductInformationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductGpu",
                table: "ProductGpu",
                column: "ProductGpuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCpu",
                table: "ProductCpu",
                column: "ProductCpuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory",
                column: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCpu_CPUs_Cpuid",
                table: "ProductCpu",
                column: "Cpuid",
                principalTable: "CPUs",
                principalColumn: "CPUId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCpu_ProductInformation_ProductInformationId",
                table: "ProductCpu",
                column: "ProductInformationId",
                principalTable: "ProductInformation",
                principalColumn: "ProductInformationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGpu_GPUs_Gpuid",
                table: "ProductGpu",
                column: "Gpuid",
                principalTable: "GPUs",
                principalColumn: "GPUId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGpu_ProductInformation_ProductInformationId",
                table: "ProductGpu",
                column: "ProductInformationId",
                principalTable: "ProductInformation",
                principalColumn: "ProductInformationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInformation_ProductCategory_ProductCategoryId",
                table: "ProductInformation",
                column: "ProductCategoryId",
                principalTable: "ProductCategory",
                principalColumn: "ProductCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRam_ProductInformation_ProductInformationId",
                table: "ProductRam",
                column: "ProductInformationId",
                principalTable: "ProductInformation",
                principalColumn: "ProductInformationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
