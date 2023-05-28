using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace BlaBlaCar.DAL.Migrations
{
    public partial class LatLon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Point>(
                name: "EndLocation",
                table: "Trips",
                type: "geography",
                nullable: true);

            migrationBuilder.AddColumn<Point>(
                name: "StartLocation",
                table: "Trips",
                type: "geography",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndLocation",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "StartLocation",
                table: "Trips");
        }
    }
}
