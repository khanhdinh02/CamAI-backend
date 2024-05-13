using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreSupervisor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HeadSupervisorId",
                table: "Incidents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_HeadSupervisorId",
                table: "Incidents",
                column: "HeadSupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Accounts_HeadSupervisorId",
                table: "Incidents",
                column: "HeadSupervisorId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Accounts_HeadSupervisorId",
                table: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_HeadSupervisorId",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "HeadSupervisorId",
                table: "Incidents");
        }
    }
}
