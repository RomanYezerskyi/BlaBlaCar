using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlaBlaCar.DAL.Migrations
{
    public partial class RenamedColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Num",
                table: "Seats",
                newName: "SeatNumber");

            migrationBuilder.RenameColumn(
                name: "RegistNum",
                table: "Cars",
                newName: "RegistrationNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_RegistNum",
                table: "Cars",
                newName: "IX_Cars_RegistrationNumber");

            migrationBuilder.RenameColumn(
                name: "TechPassport",
                table: "CarDocuments",
                newName: "TechnicalPassport");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SeatNumber",
                table: "Seats",
                newName: "Num");

            migrationBuilder.RenameColumn(
                name: "RegistrationNumber",
                table: "Cars",
                newName: "RegistNum");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_RegistrationNumber",
                table: "Cars",
                newName: "IX_Cars_RegistNum");

            migrationBuilder.RenameColumn(
                name: "TechnicalPassport",
                table: "CarDocuments",
                newName: "TechPassport");
        }
    }
}
