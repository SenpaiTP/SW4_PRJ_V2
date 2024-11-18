using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRJ4.Migrations
{
    /// <inheritdoc />
    public partial class Add_Bruger : Migration
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

            migrationBuilder.CreateIndex(
                name: "IX_Vudgifters_BrugerId",
                table: "Vudgifters",
                column: "BrugerId");

            migrationBuilder.CreateIndex(
                name: "IX_Fudgifters_BrugerId",
                table: "Fudgifters",
                column: "BrugerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fudgifters_Brugers_BrugerId",
                table: "Fudgifters",
                column: "BrugerId",
                principalTable: "Brugers",
                principalColumn: "BrugerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vudgifters_Brugers_BrugerId",
                table: "Vudgifters",
                column: "BrugerId",
                principalTable: "Brugers",
                principalColumn: "BrugerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fudgifters_Brugers_BrugerId",
                table: "Fudgifters");

            migrationBuilder.DropForeignKey(
                name: "FK_Vudgifters_Brugers_BrugerId",
                table: "Vudgifters");

            migrationBuilder.DropTable(
                name: "Brugers");

            migrationBuilder.DropIndex(
                name: "IX_Vudgifters_BrugerId",
                table: "Vudgifters");

            migrationBuilder.DropIndex(
                name: "IX_Fudgifters_BrugerId",
                table: "Fudgifters");
        }
    }
}
