using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlaBlaCar.DAL.Migrations
{
    public partial class TripCar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Cars_CarId",
                table: "Trips");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Cars_CarId",
                table: "Trips",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Cars_CarId",
                table: "Trips");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Cars_CarId",
                table: "Trips",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
