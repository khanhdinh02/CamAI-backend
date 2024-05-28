using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSupervisorAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupervisorAssignments_Accounts_HeadSupervisorId",
                table: "SupervisorAssignments");

            migrationBuilder.DropIndex(
                name: "IX_SupervisorAssignments_HeadSupervisorId",
                table: "SupervisorAssignments");

            migrationBuilder.DropColumn(
                name: "HeadSupervisorId",
                table: "SupervisorAssignments");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignmentId",
                table: "Incidents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_AssignmentId",
                table: "Incidents",
                column: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_SupervisorAssignments_AssignmentId",
                table: "Incidents",
                column: "AssignmentId",
                principalTable: "SupervisorAssignments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_SupervisorAssignments_AssignmentId",
                table: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_AssignmentId",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Incidents");

            migrationBuilder.AddColumn<Guid>(
                name: "HeadSupervisorId",
                table: "SupervisorAssignments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorAssignments_HeadSupervisorId",
                table: "SupervisorAssignments",
                column: "HeadSupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupervisorAssignments_Accounts_HeadSupervisorId",
                table: "SupervisorAssignments",
                column: "HeadSupervisorId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
