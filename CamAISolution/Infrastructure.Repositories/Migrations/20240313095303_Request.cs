using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Request : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EdgeBoxId",
                table: "Requests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_EdgeBoxId",
                table: "Requests",
                column: "EdgeBoxId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_EdgeBoxes_EdgeBoxId",
                table: "Requests",
                column: "EdgeBoxId",
                principalTable: "EdgeBoxes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_EdgeBoxes_EdgeBoxId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_EdgeBoxId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "EdgeBoxId",
                table: "Requests");
        }
    }
}
