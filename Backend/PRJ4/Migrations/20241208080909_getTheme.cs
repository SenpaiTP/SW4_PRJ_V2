using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRJ4.Migrations
{
    /// <inheritdoc />
    public partial class getTheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrugerId",
                table: "Indstillingers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Indstillingers_BrugerId",
                table: "Indstillingers",
                column: "BrugerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Indstillingers_AspNetUsers_BrugerId",
                table: "Indstillingers",
                column: "BrugerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Indstillingers_AspNetUsers_BrugerId",
                table: "Indstillingers");

            migrationBuilder.DropIndex(
                name: "IX_Indstillingers_BrugerId",
                table: "Indstillingers");

            migrationBuilder.DropColumn(
                name: "BrugerId",
                table: "Indstillingers");
        }
    }
}
