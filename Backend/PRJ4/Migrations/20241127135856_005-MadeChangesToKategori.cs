using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRJ4.Migrations
{
    /// <inheritdoc />
<<<<<<<< HEAD:Backend/PRJ4/Migrations/20241127135856_005-MadeChangesToKategori.cs
    public partial class _005MadeChangesToKategori : Migration
========
    public partial class _004ChangedKategoriTable : Migration
>>>>>>>> feature/Vindtægt:Backend/PRJ4/Migrations/20241128080240_004-ChangedKategoriTable.cs
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Navn",
                table: "Kategorier",
                newName: "KategoriNavn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KategoriNavn",
                table: "Kategorier",
                newName: "Navn");
        }
    }
}
