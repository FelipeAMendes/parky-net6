using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkyApi.Migrations
{
    public partial class UpdateEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Diffucult",
                table: "Trail",
                newName: "Difficult");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Difficult",
                table: "Trail",
                newName: "Diffucult");
        }
    }
}
