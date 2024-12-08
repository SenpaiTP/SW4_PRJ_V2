using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRJ4.Migrations
{
    /// <inheritdoc />
    public partial class _002AddedVariableIndtægter : Migration
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
                name: "Vindtægter",
                columns: table => new
                {
                    VindtægtId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrugerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tekst = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Indtægt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Dato = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vindtægter", x => x.VindtægtId);
                    table.ForeignKey(
                        name: "FK_Vindtægter_AspNetUsers_BrugerId",
                        column: x => x.BrugerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
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
                name: "IX_Vindtægter_BrugerId",
                table: "Vindtægter",
                column: "BrugerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropTable(
                name: "Vindtægter");
        }
    }
}
