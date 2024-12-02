using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRJ4.Migrations
{
    /// <inheritdoc />
    public partial class _003AddedKategoriToVariableIndtægt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KategoriId",
                table: "Vindtægter",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vindtægter_KategoriId",
                table: "Vindtægter",
                column: "KategoriId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vindtægter_Kategorier_KategoriId",
                table: "Vindtægter",
                column: "KategoriId",
                principalTable: "Kategorier",
                principalColumn: "KategoriId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vindtægter_Kategorier_KategoriId",
                table: "Vindtægter");

            migrationBuilder.DropIndex(
                name: "IX_Vindtægter_KategoriId",
                table: "Vindtægter");

            migrationBuilder.DropColumn(
                name: "KategoriId",
                table: "Vindtægter");
        }
    }
}
