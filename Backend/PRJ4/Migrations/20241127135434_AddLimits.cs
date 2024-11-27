using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRJ4.Migrations
{
    /// <inheritdoc />
    public partial class AddLimits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    BudgetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrugerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SavingsGoal = table.Column<int>(type: "int", nullable: false),
                    BudgetStart = table.Column<DateOnly>(type: "date", nullable: false),
                    BudgetSlut = table.Column<DateOnly>(type: "date", nullable: false),
                    BrugerId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.BudgetId);
                    table.ForeignKey(
                        name: "FK_Budgets_AspNetUsers_BrugerId",
                        column: x => x.BrugerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Budgets_Brugers_BrugerId1",
                        column: x => x.BrugerId1,
                        principalTable: "Brugers",
                        principalColumn: "BrugerId");
                });

            migrationBuilder.CreateTable(
                name: "KategoryLimits",
                columns: table => new
                {
                    KategoryLimitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrugerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    KategoriId = table.Column<int>(type: "int", nullable: false),
                    Limit = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KategoryLimits", x => x.KategoryLimitId);
                    table.ForeignKey(
                        name: "FK_KategoryLimits_AspNetUsers_BrugerId",
                        column: x => x.BrugerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KategoryLimits_Kategorier_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "Kategorier",
                        principalColumn: "KategoriId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_BrugerId",
                table: "Budgets",
                column: "BrugerId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_BrugerId1",
                table: "Budgets",
                column: "BrugerId1");

            migrationBuilder.CreateIndex(
                name: "IX_KategoryLimits_BrugerId",
                table: "KategoryLimits",
                column: "BrugerId");

            migrationBuilder.CreateIndex(
                name: "IX_KategoryLimits_KategoriId",
                table: "KategoryLimits",
                column: "KategoriId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropTable(
                name: "KategoryLimits");
        }
    }
}
