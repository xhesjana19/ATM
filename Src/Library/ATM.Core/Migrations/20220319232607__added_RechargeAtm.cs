using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ATM.Core.Migrations
{
    public partial class _added_RechargeAtm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RechargeATM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Banknotes5000 = table.Column<int>(type: "int", nullable: true),
                    Banknotes2000 = table.Column<int>(type: "int", nullable: true),
                    Banknotes1000 = table.Column<int>(type: "int", nullable: true),
                    Banknotes500 = table.Column<int>(type: "int", nullable: true),
                    AtmId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RechargeATM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RechargeATM_ATM_AtmId",
                        column: x => x.AtmId,
                        principalTable: "ATM",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RechargeATM_AtmId",
                table: "RechargeATM",
                column: "AtmId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RechargeATM");
        }
    }
}
