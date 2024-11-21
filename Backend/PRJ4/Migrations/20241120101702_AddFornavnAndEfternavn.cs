using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRJ4.Migrations
{
    /// <inheritdoc />
    public partial class AddFornavnAndEfternavn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Navn",
                table: "Brugers",
                newName: "Fornavn");

            migrationBuilder.AddColumn<string>(
                name: "Efternavn",
                table: "Brugers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Efternavn",
                table: "Brugers");

            migrationBuilder.RenameColumn(
                name: "Fornavn",
                table: "Brugers",
                newName: "Navn");
        }
    }
}
