using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddEdgeBoxStuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastSeen",
                table: "EdgeBoxInstalls",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperatingSystem",
                table: "EdgeBoxInstalls",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MacAddress",
                table: "EdgeBoxes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "EdgeBoxes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSeen",
                table: "EdgeBoxInstalls");

            migrationBuilder.DropColumn(
                name: "OperatingSystem",
                table: "EdgeBoxInstalls");

            migrationBuilder.DropColumn(
                name: "MacAddress",
                table: "EdgeBoxes");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "EdgeBoxes");
        }
    }
}
