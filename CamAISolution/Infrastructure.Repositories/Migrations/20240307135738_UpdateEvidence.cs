using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEvidence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evidences_EdgeBoxes_EdgeBoxId",
                table: "Evidences");

            migrationBuilder.DropIndex(
                name: "IX_Evidences_EdgeBoxId",
                table: "Evidences");

            migrationBuilder.DropColumn(
                name: "EdgeBoxId",
                table: "Evidences");

            migrationBuilder.AddColumn<Guid>(
                name: "EdgeBoxId",
                table: "Incidents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Incidents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Evidences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_EdgeBoxId",
                table: "Incidents",
                column: "EdgeBoxId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_EdgeBoxes_EdgeBoxId",
                table: "Incidents",
                column: "EdgeBoxId",
                principalTable: "EdgeBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_EdgeBoxes_EdgeBoxId",
                table: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_EdgeBoxId",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "EdgeBoxId",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Evidences");

            migrationBuilder.AddColumn<Guid>(
                name: "EdgeBoxId",
                table: "Evidences",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_EdgeBoxId",
                table: "Evidences",
                column: "EdgeBoxId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evidences_EdgeBoxes_EdgeBoxId",
                table: "Evidences",
                column: "EdgeBoxId",
                principalTable: "EdgeBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
