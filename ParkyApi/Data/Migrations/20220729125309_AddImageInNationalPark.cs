using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkyApi.Migrations
{
    public partial class AddImageInNationalPark : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Picture",
                table: "NationalPark",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "NationalPark");
        }
    }
}
