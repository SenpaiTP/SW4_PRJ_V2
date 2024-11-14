using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRJ4.Migrations
{
    /// <inheritdoc />
    public partial class InitKategorier_updateVudgfiter_updateFudgifter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_Vudgifters_KategoriId",
                table: "Vudgifters",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_Fudgifters_KategoriId",
                table: "Fudgifters",
                column: "KategoriId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fudgifters_Kategorier_KategoriId",
                table: "Fudgifters",
                column: "KategoriId",
                principalTable: "Kategorier",
                principalColumn: "KategoriId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vudgifters_Kategorier_KategoriId",
                table: "Vudgifters",
                column: "KategoriId",
                principalTable: "Kategorier",
                principalColumn: "KategoriId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fudgifters_Kategorier_KategoriId",
                table: "Fudgifters");

            migrationBuilder.DropForeignKey(
                name: "FK_Vudgifters_Kategorier_KategoriId",
                table: "Vudgifters");

            migrationBuilder.DropTable(
                name: "Kategorier");

            migrationBuilder.DropIndex(
                name: "IX_Vudgifters_KategoriId",
                table: "Vudgifters");

            migrationBuilder.DropIndex(
                name: "IX_Fudgifters_KategoriId",
                table: "Fudgifters");
        }
    }
}
