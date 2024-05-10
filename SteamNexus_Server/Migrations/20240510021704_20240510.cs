using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteamNexus_Server.Migrations
{
    /// <inheritdoc />
    public partial class _20240510 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Advertisements",
                columns: table => new
                {
                    AdvertisementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsShow = table.Column<bool>(type: "bit", nullable: false),
                    Discription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisements", x => x.AdvertisementId);
                });

            migrationBuilder.CreateTable(
                name: "CommonQuestions",
                columns: table => new
                {
                    CommonQuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonQuestions", x => x.CommonQuestionId);
                });

            migrationBuilder.CreateTable(
                name: "ComputerPartCategories",
                columns: table => new
                {
                    ComputerPartCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputerPartCategories", x => x.ComputerPartCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "CPUs",
                columns: table => new
                {
                    CPUId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPUs", x => x.CPUId);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    AppId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OriginalPrice = table.Column<int>(type: "int", nullable: true),
                    CurrentPrice = table.Column<int>(type: "int", nullable: true),
                    LowestPrice = table.Column<int>(type: "int", nullable: true),
                    AgeRating = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CommentNum = table.Column<int>(type: "int", maxLength: 100, nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Publisher = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Players = table.Column<int>(type: "int", nullable: true),
                    PeakPlayers = table.Column<int>(type: "int", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    VideoPath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                });

            migrationBuilder.CreateTable(
                name: "GPUs",
                columns: table => new
                {
                    GPUId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GPUs", x => x.GPUId);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.LanguageId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "ComponentClassifications",
                columns: table => new
                {
                    ComponentClassificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    ComputerPartCategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentClassifications", x => x.ComponentClassificationId);
                    table.ForeignKey(
                        name: "FK_ComponentClassifications_ComputerPartCategories",
                        column: x => x.ComputerPartCategoryId,
                        principalTable: "ComputerPartCategories",
                        principalColumn: "ComputerPartCategoryId");
                });

            migrationBuilder.CreateTable(
                name: "PlayersHistories",
                columns: table => new
                {
                    PlayersHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Players = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayersHistories", x => x.PlayersHistoryId);
                    table.ForeignKey(
                        name: "FK_PlayersHistories_Games",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId");
                });

            migrationBuilder.CreateTable(
                name: "PriceHistories",
                columns: table => new
                {
                    PriceHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceHistories", x => x.PriceHistoryId);
                    table.ForeignKey(
                        name: "FK_PriceHistories_Games",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId");
                });

            migrationBuilder.CreateTable(
                name: "MinimumRequirements",
                columns: table => new
                {
                    MinReqId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    CPUId = table.Column<int>(type: "int", nullable: false),
                    GPUId = table.Column<int>(type: "int", nullable: false),
                    RAM = table.Column<int>(type: "int", nullable: false),
                    OS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DirectX = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Network = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Storage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Audio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinimumRequirements", x => x.MinReqId);
                    table.ForeignKey(
                        name: "FK_MinimumRequirements_CPUs",
                        column: x => x.CPUId,
                        principalTable: "CPUs",
                        principalColumn: "CPUId");
                    table.ForeignKey(
                        name: "FK_MinimumRequirements_GPUs",
                        column: x => x.GPUId,
                        principalTable: "GPUs",
                        principalColumn: "GPUId");
                    table.ForeignKey(
                        name: "FK_MinimumRequirements_Games",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId");
                });

            migrationBuilder.CreateTable(
                name: "RecommendedRequirements",
                columns: table => new
                {
                    RecReqId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    CPUId = table.Column<int>(type: "int", nullable: false),
                    GPUId = table.Column<int>(type: "int", nullable: false),
                    RAM = table.Column<int>(type: "int", nullable: false),
                    OS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DirectX = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Network = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Storage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Audio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendedRequirements", x => x.RecReqId);
                    table.ForeignKey(
                        name: "FK_RecommendedRequirements_CPUs",
                        column: x => x.CPUId,
                        principalTable: "CPUs",
                        principalColumn: "CPUId");
                    table.ForeignKey(
                        name: "FK_RecommendedRequirements_GPUs",
                        column: x => x.GPUId,
                        principalTable: "GPUs",
                        principalColumn: "GPUId");
                    table.ForeignKey(
                        name: "FK_RecommendedRequirements_Games",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId");
                });

            migrationBuilder.CreateTable(
                name: "GameLanguages",
                columns: table => new
                {
                    GameLanguageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Support = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLanguages", x => x.GameLanguageId);
                    table.ForeignKey(
                        name: "FK_GameLanguages_Games",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId");
                    table.ForeignKey(
                        name: "FK_GameLanguages_Languages",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Photo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "TagGroups",
                columns: table => new
                {
                    TagGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagGroups", x => x.TagGroupId);
                    table.ForeignKey(
                        name: "FK_TagGroups_Games",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId");
                    table.ForeignKey(
                        name: "FK_TagGroups_Tags",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId");
                });

            migrationBuilder.CreateTable(
                name: "ProductInformations",
                columns: table => new
                {
                    ProductInformationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    ComponentClassificationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Specification = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Wattage = table.Column<int>(type: "int", nullable: false),
                    Recommend = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInformations", x => x.ProductInformationId);
                    table.ForeignKey(
                        name: "FK_ProductInformations_ComponentClassifications",
                        column: x => x.ComponentClassificationId,
                        principalTable: "ComponentClassifications",
                        principalColumn: "ComponentClassificationId");
                });

            migrationBuilder.CreateTable(
                name: "ProductCPUs",
                columns: table => new
                {
                    ProductCPUId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    ProductInformationId = table.Column<int>(type: "int", nullable: false),
                    CPUId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCPUs", x => x.ProductCPUId);
                    table.ForeignKey(
                        name: "FK_ProductCPUs_CPUs",
                        column: x => x.CPUId,
                        principalTable: "CPUs",
                        principalColumn: "CPUId");
                    table.ForeignKey(
                        name: "FK_ProductCPUs_ProductInformations",
                        column: x => x.ProductInformationId,
                        principalTable: "ProductInformations",
                        principalColumn: "ProductInformationId");
                });

            migrationBuilder.CreateTable(
                name: "ProductGPUs",
                columns: table => new
                {
                    ProductGPUId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    ProductInformationId = table.Column<int>(type: "int", nullable: false),
                    GPUId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGPUs", x => x.ProductGPUId);
                    table.ForeignKey(
                        name: "FK_ProductGPUs_GPUs",
                        column: x => x.GPUId,
                        principalTable: "GPUs",
                        principalColumn: "GPUId");
                    table.ForeignKey(
                        name: "FK_ProductGPUs_ProductInformations",
                        column: x => x.ProductInformationId,
                        principalTable: "ProductInformations",
                        principalColumn: "ProductInformationId");
                });

            migrationBuilder.CreateTable(
                name: "ProductRAMs",
                columns: table => new
                {
                    ProductRAMId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    ProductInformationId = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRAMs", x => x.ProductRAMId);
                    table.ForeignKey(
                        name: "FK_ProductRAMs_ProductInformations",
                        column: x => x.ProductInformationId,
                        principalTable: "ProductInformations",
                        principalColumn: "ProductInformationId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentClassifications_ComputerPartCategoryId",
                table: "ComponentClassifications",
                column: "ComputerPartCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLanguages_GameId",
                table: "GameLanguages",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLanguages_LanguageId",
                table: "GameLanguages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_MinimumRequirements_CPUId",
                table: "MinimumRequirements",
                column: "CPUId");

            migrationBuilder.CreateIndex(
                name: "IX_MinimumRequirements_GameId",
                table: "MinimumRequirements",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_MinimumRequirements_GPUId",
                table: "MinimumRequirements",
                column: "GPUId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayersHistories_GameId",
                table: "PlayersHistories",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistories_GameId",
                table: "PriceHistories",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCPUs_CPUId",
                table: "ProductCPUs",
                column: "CPUId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCPUs_ProductInformationId",
                table: "ProductCPUs",
                column: "ProductInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGPUs_GPUId",
                table: "ProductGPUs",
                column: "GPUId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGPUs_ProductInformationId",
                table: "ProductGPUs",
                column: "ProductInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInformations_ComponentClassificationId",
                table: "ProductInformations",
                column: "ComponentClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRAMs_ProductInformationId",
                table: "ProductRAMs",
                column: "ProductInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedRequirements_CPUId",
                table: "RecommendedRequirements",
                column: "CPUId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedRequirements_GameId",
                table: "RecommendedRequirements",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedRequirements_GPUId",
                table: "RecommendedRequirements",
                column: "GPUId");

            migrationBuilder.CreateIndex(
                name: "IX_TagGroups_GameId",
                table: "TagGroups",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_TagGroups_TagId",
                table: "TagGroups",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advertisements");

            migrationBuilder.DropTable(
                name: "CommonQuestions");

            migrationBuilder.DropTable(
                name: "GameLanguages");

            migrationBuilder.DropTable(
                name: "MinimumRequirements");

            migrationBuilder.DropTable(
                name: "PlayersHistories");

            migrationBuilder.DropTable(
                name: "PriceHistories");

            migrationBuilder.DropTable(
                name: "ProductCPUs");

            migrationBuilder.DropTable(
                name: "ProductGPUs");

            migrationBuilder.DropTable(
                name: "ProductRAMs");

            migrationBuilder.DropTable(
                name: "RecommendedRequirements");

            migrationBuilder.DropTable(
                name: "TagGroups");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "ProductInformations");

            migrationBuilder.DropTable(
                name: "CPUs");

            migrationBuilder.DropTable(
                name: "GPUs");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "ComponentClassifications");

            migrationBuilder.DropTable(
                name: "ComputerPartCategories");
        }
    }
}
