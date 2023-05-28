using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlaBlaCar.DAL.Migrations
{
    public partial class ReadMessagesKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReadMessages",
                table: "ReadMessages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReadMessages",
                table: "ReadMessages",
                columns: new[] { "Id", "MessageId", "UserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReadMessages",
                table: "ReadMessages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReadMessages",
                table: "ReadMessages",
                column: "Id");
        }
    }
}
