using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ATM.Core.Migrations
{
    public partial class _added_atmWithdrawals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ATMWithdrawals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    AtmId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Banknotes5000 = table.Column<int>(type: "int", nullable: true),
                    Banknotes2000 = table.Column<int>(type: "int", nullable: true),
                    Banknotes1000 = table.Column<int>(type: "int", nullable: true),
                    Banknotes500 = table.Column<int>(type: "int", nullable: true),
                    AmountSet = table.Column<int>(type: "int", nullable: false),
                    AmountGet = table.Column<int>(type: "int", nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATMWithdrawals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ATMWithdrawals_ATM_AtmId",
                        column: x => x.AtmId,
                        principalTable: "ATM",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ATMWithdrawals_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ATMWithdrawals_AtmId",
                table: "ATMWithdrawals",
                column: "AtmId");

            migrationBuilder.CreateIndex(
                name: "IX_ATMWithdrawals_UserId",
                table: "ATMWithdrawals",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ATMWithdrawals");
        }
    }
}
