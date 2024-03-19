using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIncidentShopLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeIncident");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "Incidents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShopId",
                table: "Incidents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_EmployeeId",
                table: "Incidents",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_ShopId",
                table: "Incidents",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Employees_EmployeeId",
                table: "Incidents",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Shops_ShopId",
                table: "Incidents",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Employees_EmployeeId",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Shops_ShopId",
                table: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_EmployeeId",
                table: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_ShopId",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Incidents");

            migrationBuilder.CreateTable(
                name: "EmployeeIncident",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IncidentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeIncident", x => new { x.EmployeeId, x.IncidentId });
                    table.ForeignKey(
                        name: "FK_EmployeeIncident_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeIncident_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeIncident_IncidentId",
                table: "EmployeeIncident",
                column: "IncidentId");
        }
    }
}
