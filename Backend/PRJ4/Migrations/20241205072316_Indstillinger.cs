using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRJ4.Migrations
{
    /// <inheritdoc />
    public partial class Indstillinger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Indstillingers",
                columns: table => new
                {
                    IndstillingerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SetTheme = table.Column<bool>(type: "bit", nullable: false),
                    SetPieChart = table.Column<bool>(type: "bit", nullable: false),
                    SetSøjlediagram = table.Column<bool>(type: "bit", nullable: false),
                    SetIndtægter = table.Column<bool>(type: "bit", nullable: false),
                    SetUdgifter = table.Column<bool>(type: "bit", nullable: false),
                    SetBudget = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indstillingers", x => x.IndstillingerId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Indstillingers");
        }
    }
}
