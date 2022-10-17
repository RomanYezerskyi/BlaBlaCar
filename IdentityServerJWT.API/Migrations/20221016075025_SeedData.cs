using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityServerJWT.API.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e23e7753-c716-40d0-b91a-ca49b10e304a", "dab52546-dffd-4f80-bdc4-6f3e899e1ab6", "blablacar.admin", "BLABLACAR.ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fe0d6a7a-c600-4026-a77c-f26a9c307390", "0fb230b3-952d-4bae-b80a-8bc8d948e350", "blablacar.user", "BLABLACAR.USER" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8e445865-a24d-4543-a6c6-9443d048cdb9", 0, "7eebc6ce-8980-4050-8f9f-a3d271c27cdd", "admin@gmail.com", true, "admin", false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAEAACcQAAAAEHB/ECWGtBKZm3E3RFXmkWOs2es/siVa2At+ZWHP1Lrbkot2h6fbHoaSiODnpt2WKQ==", "+380999999999", false, null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "f391339f-4ced-47e2-8abc-fb845ec7f56d", false, "admin@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 1, "name", "admin@gmail.com", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { 2, "id", "8e445865-a24d-4543-a6c6-9443d048cdb9", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { 3, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "admin@gmail.com", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { 4, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", "admin", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { 5, "phone_number", "+380999999999", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { 6, "role", "blablacar.admin", "8e445865-a24d-4543-a6c6-9443d048cdb9" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "e23e7753-c716-40d0-b91a-ca49b10e304a", "8e445865-a24d-4543-a6c6-9443d048cdb9" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fe0d6a7a-c600-4026-a77c-f26a9c307390");

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e23e7753-c716-40d0-b91a-ca49b10e304a", "8e445865-a24d-4543-a6c6-9443d048cdb9" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e23e7753-c716-40d0-b91a-ca49b10e304a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9");
        }
    }
}
