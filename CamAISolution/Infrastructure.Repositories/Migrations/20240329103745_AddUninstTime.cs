using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddUninstTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EdgeBoxPath",
                table: "Evidences");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Evidences");

            migrationBuilder.AddColumn<DateTime>(
                name: "UninstalledTime",
                table: "EdgeBoxInstalls",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UninstalledTime",
                table: "EdgeBoxInstalls");

            migrationBuilder.AddColumn<string>(
                name: "EdgeBoxPath",
                table: "Evidences",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Evidences",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
