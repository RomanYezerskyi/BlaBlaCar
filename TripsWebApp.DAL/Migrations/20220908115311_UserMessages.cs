using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlaBlaCar.DAL.Migrations
{
    public partial class UserMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReadMessages_UsersInChats_UsersInChatsId",
                table: "ReadMessages");

            migrationBuilder.RenameColumn(
                name: "UsersInChatsId",
                table: "ReadMessages",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ReadMessages_UsersInChatsId",
                table: "ReadMessages",
                newName: "IX_ReadMessages_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReadMessages_Users_UserId",
                table: "ReadMessages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReadMessages_Users_UserId",
                table: "ReadMessages");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ReadMessages",
                newName: "UsersInChatsId");

            migrationBuilder.RenameIndex(
                name: "IX_ReadMessages_UserId",
                table: "ReadMessages",
                newName: "IX_ReadMessages_UsersInChatsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReadMessages_UsersInChats_UsersInChatsId",
                table: "ReadMessages",
                column: "UsersInChatsId",
                principalTable: "UsersInChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
