using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRJ4.Migrations
{
    /// <inheritdoc />
    public partial class _004AddedKategoriTilIndtægt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KategoriId",
                table: "Vindtægter",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KategoriId",
                table: "Findtægter",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vindtægter_KategoriId",
                table: "Vindtægter",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_Findtægter_KategoriId",
                table: "Findtægter",
                column: "KategoriId");

            migrationBuilder.AddForeignKey(
                name: "FK_Findtægter_Kategorier_KategoriId",
                table: "Findtægter",
                column: "KategoriId",
                principalTable: "Kategorier",
                principalColumn: "KategoriId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vindtægter_Kategorier_KategoriId",
                table: "Vindtægter",
                column: "KategoriId",
                principalTable: "Kategorier",
                principalColumn: "KategoriId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Findtægter_Kategorier_KategoriId",
                table: "Findtægter");

            migrationBuilder.DropForeignKey(
                name: "FK_Vindtægter_Kategorier_KategoriId",
                table: "Vindtægter");

            migrationBuilder.DropIndex(
                name: "IX_Vindtægter_KategoriId",
                table: "Vindtægter");

            migrationBuilder.DropIndex(
                name: "IX_Findtægter_KategoriId",
                table: "Findtægter");

            migrationBuilder.DropColumn(
                name: "KategoriId",
                table: "Vindtægter");

            migrationBuilder.DropColumn(
                name: "KategoriId",
                table: "Findtægter");
        }
    }
}
