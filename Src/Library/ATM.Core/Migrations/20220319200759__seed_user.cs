using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ATM.Core.Migrations
{
    public partial class _seed_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
       table: "User",
           columns: new[] { "Id", "CreatedById", "CreatedOnUtc", "Name", "Password", "Role", "UpdatedById", "UpdatedOnUtc", "Username", "IsActive", "PasswordCipher", "IsDeleted" },
                values: new object[] { new Guid("89F2B01E-D8A8-EC11-83C7-ECB1D797E5CD"), new Guid("89F2B01E-D8A8-EC11-83C7-ECB1D797E5CD"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", "3C7959E8355F19CB6C7A023E46099E5EA9EF23CC4C75675D153B366289FA1D1DF18134229825B75064C6A4E86D97E3FA6EBAAED2C1DA8C93500024C3C3F4FFD4", "Administrator", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "test@gmail.com", true, "Test@123", false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
