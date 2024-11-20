using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRJ4.Migrations
{
    /// <inheritdoc />
    public partial class migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brugers",
                columns: table => new
                {
                    BrugerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brugers", x => x.BrugerId);
                });

            migrationBuilder.CreateTable(
                name: "Kategorier",
                columns: table => new
                {
                    KategoriId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorier", x => x.KategoriId);
                });

            migrationBuilder.CreateTable(
                name: "LoginModels",
                columns: table => new
                {
                    LoginId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginModels", x => x.LoginId);
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    BudgetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrugerId = table.Column<int>(type: "int", nullable: true),
                    SavingsGoal = table.Column<int>(type: "int", nullable: false),
                    BudgetStart = table.Column<DateOnly>(type: "date", nullable: false),
                    BudgetSlut = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.BudgetId);
                    table.ForeignKey(
                        name: "FK_Budgets_Brugers_BrugerId",
                        column: x => x.BrugerId,
                        principalTable: "Brugers",
                        principalColumn: "BrugerId");
                });

            migrationBuilder.CreateTable(
                name: "Fudgifters",
                columns: table => new
                {
                    FudgiftId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pris = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tekst = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dato = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KategoriId = table.Column<int>(type: "int", nullable: false),
                    BrugerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fudgifters", x => x.FudgiftId);
                    table.ForeignKey(
                        name: "FK_Fudgifters_Brugers_BrugerId",
                        column: x => x.BrugerId,
                        principalTable: "Brugers",
                        principalColumn: "BrugerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fudgifters_Kategorier_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "Kategorier",
                        principalColumn: "KategoriId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vudgifters",
                columns: table => new
                {
                    VudgiftId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pris = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tekst = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dato = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BrugerId = table.Column<int>(type: "int", nullable: false),
                    KategoriId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vudgifters", x => x.VudgiftId);
                    table.ForeignKey(
                        name: "FK_Vudgifters_Brugers_BrugerId",
                        column: x => x.BrugerId,
                        principalTable: "Brugers",
                        principalColumn: "BrugerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vudgifters_Kategorier_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "Kategorier",
                        principalColumn: "KategoriId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_BrugerId",
                table: "Budgets",
                column: "BrugerId");

            migrationBuilder.CreateIndex(
                name: "IX_Fudgifters_BrugerId",
                table: "Fudgifters",
                column: "BrugerId");

            migrationBuilder.CreateIndex(
                name: "IX_Fudgifters_KategoriId",
                table: "Fudgifters",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_Vudgifters_BrugerId",
                table: "Vudgifters",
                column: "BrugerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vudgifters_KategoriId",
                table: "Vudgifters",
                column: "KategoriId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropTable(
                name: "Fudgifters");

            migrationBuilder.DropTable(
                name: "LoginModels");

            migrationBuilder.DropTable(
                name: "Vudgifters");

            migrationBuilder.DropTable(
                name: "Brugers");

            migrationBuilder.DropTable(
                name: "Kategorier");
        }
    }
}
