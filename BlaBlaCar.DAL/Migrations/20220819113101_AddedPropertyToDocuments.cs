using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlaBlaCar.DAL.Migrations
{
    public partial class AddedPropertyToDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Seats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Seats",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Seats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "Seats",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CarDocuments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "CarDocuments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CarDocuments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "CarDocuments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AvailableSeats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "AvailableSeats",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AvailableSeats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "AvailableSeats",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CarDocuments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CarDocuments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CarDocuments");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CarDocuments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AvailableSeats");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AvailableSeats");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AvailableSeats");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AvailableSeats");
        }
    }
}
