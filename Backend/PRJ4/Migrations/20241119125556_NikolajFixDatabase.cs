using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRJ4.Migrations
{
    /// <inheritdoc />
    public partial class NikolajFixDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Findtægter",
                columns: table => new
                {
                    FindtægtId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrugerId = table.Column<int>(type: "int", nullable: false),
                    Tekst = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Indtægt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Dato = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Findtægter", x => x.FindtægtId);
                    table.ForeignKey(
                        name: "FK_Findtægter_Brugers_BrugerId",
                        column: x => x.BrugerId,
                        principalTable: "Brugers",
                        principalColumn: "BrugerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Findtægter_BrugerId",
                table: "Findtægter",
                column: "BrugerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Findtægter");
        }
    }
}
