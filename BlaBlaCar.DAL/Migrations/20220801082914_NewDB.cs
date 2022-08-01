using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlaBlaCar.DAL.Migrations
{
    public partial class NewDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Trips_TripId",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_userId",
                table: "Trips");

            migrationBuilder.DropTable(
                name: "BookedSeats");

            migrationBuilder.DropTable(
                name: "BookedTrips");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Trips",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_userId",
                table: "Trips",
                newName: "IX_Trips_UserId");

            migrationBuilder.RenameColumn(
                name: "TripId",
                table: "Seats",
                newName: "CarId");

            migrationBuilder.RenameIndex(
                name: "IX_Seats_TripId",
                table: "Seats",
                newName: "IX_Seats_CarId");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AvailableSeats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailableSeats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvailableSeats_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvailableSeats_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cars_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SeatId = table.Column<int>(type: "int", nullable: false),
                    TripId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripUsers_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripUsers_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripUsers_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_CarId",
                table: "Trips",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailableSeats_SeatId",
                table: "AvailableSeats",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailableSeats_TripId",
                table: "AvailableSeats",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_UserId",
                table: "Cars",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TripUsers_SeatId",
                table: "TripUsers",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_TripUsers_TripId",
                table: "TripUsers",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_TripUsers_userId",
                table: "TripUsers",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Cars_CarId",
                table: "Seats",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Cars_CarId",
                table: "Trips",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_UserId",
                table: "Trips",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Cars_CarId",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Cars_CarId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_UserId",
                table: "Trips");

            migrationBuilder.DropTable(
                name: "AvailableSeats");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "TripUsers");

            migrationBuilder.DropIndex(
                name: "IX_Trips_CarId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Trips");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Trips",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_UserId",
                table: "Trips",
                newName: "IX_Trips_userId");

            migrationBuilder.RenameColumn(
                name: "CarId",
                table: "Seats",
                newName: "TripId");

            migrationBuilder.RenameIndex(
                name: "IX_Seats_CarId",
                table: "Seats",
                newName: "IX_Seats_TripId");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BookedTrips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookedTrips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookedTrips_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookedTrips_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookedSeats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookedTripId = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookedSeats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookedSeats_BookedTrips_BookedTripId",
                        column: x => x.BookedTripId,
                        principalTable: "BookedTrips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookedSeats_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookedSeats_BookedTripId",
                table: "BookedSeats",
                column: "BookedTripId");

            migrationBuilder.CreateIndex(
                name: "IX_BookedSeats_SeatId",
                table: "BookedSeats",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_BookedTrips_TripId",
                table: "BookedTrips",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_BookedTrips_UserId",
                table: "BookedTrips",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Trips_TripId",
                table: "Seats",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_userId",
                table: "Trips",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
