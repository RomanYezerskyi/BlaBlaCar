using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlaBlaCar.DAL.Migrations
{
    public partial class AddedTripAutoStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AutoTripStatus",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoTripStatus",
                table: "Trips");
        }
    }
}
