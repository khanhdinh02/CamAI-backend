using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class EdgeBoxVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountRole",
                keyColumns: new[] { "AccountId", "RoleId" },
                keyValues: new object[] { new Guid("4b077561-b84d-47bf-ab5e-68edf456cc8d"), 1 });

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("4b077561-b84d-47bf-ab5e-68edf456cc8d"));

            migrationBuilder.AddColumn<int>(
                name: "EdgeBoxLocationId",
                table: "EdgeBoxes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "EdgeBoxes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EdgeBoxLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdgeBoxLocation", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountStatusId", "AddressLine", "Birthday", "CreatedDate", "Email", "Gender", "ModifiedDate", "Name", "Password", "Phone", "WardId", "WorkingShopId" },
                values: new object[] { new Guid("327b814e-7542-4a2b-8e0d-21ba0c703ad5"), 2, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@camai.com", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "9eb622419ace52f259e858a7f2a10743d35e36fe0d22fc2d224c320cbc68d3af", null, null, null });

            migrationBuilder.InsertData(
                table: "EdgeBoxLocation",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Idle" },
                    { 2, null, "Installing" },
                    { 3, null, "Occupied" },
                    { 4, null, "Uninstalling" },
                    { 5, null, "Disposed" }
                });

            migrationBuilder.InsertData(
                table: "EdgeBoxStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 3, null, "Broken" });

            migrationBuilder.InsertData(
                table: "AccountRole",
                columns: new[] { "AccountId", "RoleId" },
                values: new object[] { new Guid("327b814e-7542-4a2b-8e0d-21ba0c703ad5"), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_EdgeBoxes_EdgeBoxLocationId",
                table: "EdgeBoxes",
                column: "EdgeBoxLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_EdgeBoxes_EdgeBoxLocation_EdgeBoxLocationId",
                table: "EdgeBoxes",
                column: "EdgeBoxLocationId",
                principalTable: "EdgeBoxLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EdgeBoxes_EdgeBoxLocation_EdgeBoxLocationId",
                table: "EdgeBoxes");

            migrationBuilder.DropTable(
                name: "EdgeBoxLocation");

            migrationBuilder.DropIndex(
                name: "IX_EdgeBoxes_EdgeBoxLocationId",
                table: "EdgeBoxes");

            migrationBuilder.DeleteData(
                table: "AccountRole",
                keyColumns: new[] { "AccountId", "RoleId" },
                keyValues: new object[] { new Guid("327b814e-7542-4a2b-8e0d-21ba0c703ad5"), 1 });

            migrationBuilder.DeleteData(
                table: "EdgeBoxStatuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("327b814e-7542-4a2b-8e0d-21ba0c703ad5"));

            migrationBuilder.DropColumn(
                name: "EdgeBoxLocationId",
                table: "EdgeBoxes");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "EdgeBoxes");

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountStatusId", "AddressLine", "Birthday", "CreatedDate", "Email", "Gender", "ModifiedDate", "Name", "Password", "Phone", "WardId", "WorkingShopId" },
                values: new object[] { new Guid("4b077561-b84d-47bf-ab5e-68edf456cc8d"), 2, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@camai.com", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "9eb622419ace52f259e858a7f2a10743d35e36fe0d22fc2d224c320cbc68d3af", null, null, null });

            migrationBuilder.InsertData(
                table: "AccountRole",
                columns: new[] { "AccountId", "RoleId" },
                values: new object[] { new Guid("4b077561-b84d-47bf-ab5e-68edf456cc8d"), 1 });
        }
    }
}
