using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteamNexus.Migrations
{
    /// <inheritdoc />
    public partial class GPUID_HotFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MinimumRequirements_GPUs",
                table: "MinimumRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_MinimumRequirements_GPUs_GPUId1",
                table: "MinimumRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendedRequirements_GPUs",
                table: "RecommendedRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendedRequirements_GPUs_GPUId1",
                table: "RecommendedRequirements");

            migrationBuilder.DropIndex(
                name: "IX_RecommendedRequirements_GPUId1",
                table: "RecommendedRequirements");

            migrationBuilder.DropIndex(
                name: "IX_MinimumRequirements_GPUId1",
                table: "MinimumRequirements");

            migrationBuilder.DropColumn(
                name: "GPUId1",
                table: "RecommendedRequirements");

            migrationBuilder.DropColumn(
                name: "GPUId1",
                table: "MinimumRequirements");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedRequirements_CPUId",
                table: "RecommendedRequirements",
                column: "CPUId");

            migrationBuilder.CreateIndex(
                name: "IX_MinimumRequirements_CPUId",
                table: "MinimumRequirements",
                column: "CPUId");

            migrationBuilder.AddForeignKey(
                name: "FK_MinimumRequirements_CPUs",
                table: "MinimumRequirements",
                column: "CPUId",
                principalTable: "CPUs",
                principalColumn: "CPUId");

            migrationBuilder.AddForeignKey(
                name: "FK_MinimumRequirements_GPUs",
                table: "MinimumRequirements",
                column: "GPUId",
                principalTable: "GPUs",
                principalColumn: "GPUId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendedRequirements_CPUs",
                table: "RecommendedRequirements",
                column: "CPUId",
                principalTable: "CPUs",
                principalColumn: "CPUId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendedRequirements_GPUs",
                table: "RecommendedRequirements",
                column: "GPUId",
                principalTable: "GPUs",
                principalColumn: "GPUId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MinimumRequirements_CPUs",
                table: "MinimumRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_MinimumRequirements_GPUs",
                table: "MinimumRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendedRequirements_CPUs",
                table: "RecommendedRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendedRequirements_GPUs",
                table: "RecommendedRequirements");

            migrationBuilder.DropIndex(
                name: "IX_RecommendedRequirements_CPUId",
                table: "RecommendedRequirements");

            migrationBuilder.DropIndex(
                name: "IX_MinimumRequirements_CPUId",
                table: "MinimumRequirements");

            migrationBuilder.AddColumn<int>(
                name: "GPUId1",
                table: "RecommendedRequirements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GPUId1",
                table: "MinimumRequirements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedRequirements_GPUId1",
                table: "RecommendedRequirements",
                column: "GPUId1");

            migrationBuilder.CreateIndex(
                name: "IX_MinimumRequirements_GPUId1",
                table: "MinimumRequirements",
                column: "GPUId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MinimumRequirements_GPUs",
                table: "MinimumRequirements",
                column: "GPUId",
                principalTable: "CPUs",
                principalColumn: "CPUId");

            migrationBuilder.AddForeignKey(
                name: "FK_MinimumRequirements_GPUs_GPUId1",
                table: "MinimumRequirements",
                column: "GPUId1",
                principalTable: "GPUs",
                principalColumn: "GPUId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendedRequirements_GPUs",
                table: "RecommendedRequirements",
                column: "GPUId",
                principalTable: "CPUs",
                principalColumn: "CPUId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendedRequirements_GPUs_GPUId1",
                table: "RecommendedRequirements",
                column: "GPUId1",
                principalTable: "GPUs",
                principalColumn: "GPUId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
