using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkyApi.Migrations
{
    public partial class AddElevationToTrail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Elevation",
                table: "Trail",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Elevation",
                table: "Trail");
        }
    }
}
