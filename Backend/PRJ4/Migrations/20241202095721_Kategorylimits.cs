using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRJ4.Migrations
{
    /// <inheritdoc />
    public partial class Kategorylimits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KategoryLimits_Kategorier_KategoriId",
                table: "KategoryLimits");

            migrationBuilder.RenameColumn(
                name: "KategoriId",
                table: "KategoryLimits",
                newName: "KategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_KategoryLimits_KategoriId",
                table: "KategoryLimits",
                newName: "IX_KategoryLimits_KategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_KategoryLimits_Kategorier_KategoryId",
                table: "KategoryLimits",
                column: "KategoryId",
                principalTable: "Kategorier",
                principalColumn: "KategoriId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KategoryLimits_Kategorier_KategoryId",
                table: "KategoryLimits");

            migrationBuilder.RenameColumn(
                name: "KategoryId",
                table: "KategoryLimits",
                newName: "KategoriId");

            migrationBuilder.RenameIndex(
                name: "IX_KategoryLimits_KategoryId",
                table: "KategoryLimits",
                newName: "IX_KategoryLimits_KategoriId");

            migrationBuilder.AddForeignKey(
                name: "FK_KategoryLimits_Kategorier_KategoriId",
                table: "KategoryLimits",
                column: "KategoriId",
                principalTable: "Kategorier",
                principalColumn: "KategoriId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
