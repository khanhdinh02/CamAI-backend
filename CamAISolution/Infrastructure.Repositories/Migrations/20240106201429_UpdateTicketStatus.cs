using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTicketStatus : Migration
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

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountStatusId", "AddressLine", "Birthday", "CreatedDate", "Email", "Gender", "ModifiedDate", "Name", "Password", "Phone", "WardId", "WorkingShopId" },
                values: new object[] { new Guid("5cf16c9d-5f81-471a-8efa-500e671933ff"), 2, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@camai.com", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "9eb622419ace52f259e858a7f2a10743d35e36fe0d22fc2d224c320cbc68d3af", null, null, null });

            migrationBuilder.UpdateData(
                table: "TicketStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "New");

            migrationBuilder.InsertData(
                table: "TicketStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 5, null, "Active" });

            migrationBuilder.InsertData(
                table: "AccountRole",
                columns: new[] { "AccountId", "RoleId" },
                values: new object[] { new Guid("5cf16c9d-5f81-471a-8efa-500e671933ff"), 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountRole",
                keyColumns: new[] { "AccountId", "RoleId" },
                keyValues: new object[] { new Guid("5cf16c9d-5f81-471a-8efa-500e671933ff"), 1 });

            migrationBuilder.DeleteData(
                table: "TicketStatuses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("5cf16c9d-5f81-471a-8efa-500e671933ff"));

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountStatusId", "AddressLine", "Birthday", "CreatedDate", "Email", "Gender", "ModifiedDate", "Name", "Password", "Phone", "WardId", "WorkingShopId" },
                values: new object[] { new Guid("4b077561-b84d-47bf-ab5e-68edf456cc8d"), 2, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@camai.com", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "9eb622419ace52f259e858a7f2a10743d35e36fe0d22fc2d224c320cbc68d3af", null, null, null });

            migrationBuilder.UpdateData(
                table: "TicketStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Open");

            migrationBuilder.InsertData(
                table: "AccountRole",
                columns: new[] { "AccountId", "RoleId" },
                values: new object[] { new Guid("4b077561-b84d-47bf-ab5e-68edf456cc8d"), 1 });
        }
    }
}
