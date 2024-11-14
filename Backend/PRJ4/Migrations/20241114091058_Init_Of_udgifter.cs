using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRJ4.Migrations
{
    /// <inheritdoc />
    public partial class Init_Of_udgifter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fudgifters");

            migrationBuilder.DropTable(
                name: "Vudgifters");
        }
    }
}
