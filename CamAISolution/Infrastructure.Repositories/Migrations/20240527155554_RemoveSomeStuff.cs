using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSomeStuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Accounts_HeadSupervisorId",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Accounts_InChargeAccountId",
                table: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_HeadSupervisorId",
                table: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_InChargeAccountId",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "HeadSupervisorId",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "InChargeAccountId",
                table: "Incidents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HeadSupervisorId",
                table: "Incidents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InChargeAccountId",
                table: "Incidents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_HeadSupervisorId",
                table: "Incidents",
                column: "HeadSupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_InChargeAccountId",
                table: "Incidents",
                column: "InChargeAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Accounts_HeadSupervisorId",
                table: "Incidents",
                column: "HeadSupervisorId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Accounts_InChargeAccountId",
                table: "Incidents",
                column: "InChargeAccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
