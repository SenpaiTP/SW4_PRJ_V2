using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRJ4.Migrations
{
    /// <inheritdoc />
    public partial class _006FixingDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Findtægter_Kategorier_KategoriId",
                table: "Findtægter");

            migrationBuilder.DropForeignKey(
                name: "FK_Vindtægter_Kategorier_KategoriId",
                table: "Vindtægter");

            migrationBuilder.AlterColumn<int>(
                name: "KategoriId",
                table: "Vindtægter",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "KategoriId",
                table: "Findtægter",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Findtægter_Kategorier_KategoriId",
                table: "Findtægter",
                column: "KategoriId",
                principalTable: "Kategorier",
                principalColumn: "KategoriId");

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
                name: "FK_Findtægter_Kategorier_KategoriId",
                table: "Findtægter");

            migrationBuilder.DropForeignKey(
                name: "FK_Vindtægter_Kategorier_KategoriId",
                table: "Vindtægter");

            migrationBuilder.AlterColumn<int>(
                name: "KategoriId",
                table: "Vindtægter",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KategoriId",
                table: "Findtægter",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
