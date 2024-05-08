using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsForIncident : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssigningAccountId",
                table: "Incidents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InChargeAccountId",
                table: "Incidents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_AssigningAccountId",
                table: "Incidents",
                column: "AssigningAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_InChargeAccountId",
                table: "Incidents",
                column: "InChargeAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Accounts_AssigningAccountId",
                table: "Incidents",
                column: "AssigningAccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Accounts_InChargeAccountId",
                table: "Incidents",
                column: "InChargeAccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Accounts_AssigningAccountId",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Accounts_InChargeAccountId",
                table: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_AssigningAccountId",
                table: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_InChargeAccountId",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "AssigningAccountId",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "InChargeAccountId",
                table: "Incidents");
        }
    }
}
