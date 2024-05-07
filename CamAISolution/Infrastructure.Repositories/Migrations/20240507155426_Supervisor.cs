using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Supervisor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Employees");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SupervisorAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssigneeRole = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupervisorAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupervisorAssignments_Accounts_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupervisorAssignments_Accounts_AssignorId",
                        column: x => x.AssignorId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupervisorAssignments_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AccountId",
                table: "Employees",
                column: "AccountId",
                unique: true,
                filter: "[AccountId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorAssignments_AssigneeId",
                table: "SupervisorAssignments",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorAssignments_AssignorId",
                table: "SupervisorAssignments",
                column: "AssignorId");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorAssignments_ShopId",
                table: "SupervisorAssignments",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Accounts_AccountId",
                table: "Employees",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Accounts_AccountId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "SupervisorAssignments");

            migrationBuilder.DropIndex(
                name: "IX_Employees_AccountId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
