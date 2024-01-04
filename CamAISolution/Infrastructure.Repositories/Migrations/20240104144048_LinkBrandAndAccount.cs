using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class LinkBrandAndAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountRole",
                keyColumns: new[] { "AccountId", "RoleId" },
                keyValues: new object[] { new Guid("a4247434-cb54-487e-ba75-42036402a949"), 1 });

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("a4247434-cb54-487e-ba75-42036402a949"));

            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ManagingShopId",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountStatusId", "AddressLine", "Birthday", "BrandId", "CreatedDate", "Email", "Gender", "ManagingShopId", "ModifiedDate", "Name", "Password", "Phone", "WardId", "WorkingShopId" },
                values: new object[] { new Guid("c8829e92-e880-4cc9-bbfe-71e4efeeb9e1"), 2, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@camai.com", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "9eb622419ace52f259e858a7f2a10743d35e36fe0d22fc2d224c320cbc68d3af", null, null, null });

            migrationBuilder.InsertData(
                table: "AccountRole",
                columns: new[] { "AccountId", "RoleId" },
                values: new object[] { new Guid("c8829e92-e880-4cc9-bbfe-71e4efeeb9e1"), 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountRole",
                keyColumns: new[] { "AccountId", "RoleId" },
                keyValues: new object[] { new Guid("c8829e92-e880-4cc9-bbfe-71e4efeeb9e1"), 1 });

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("c8829e92-e880-4cc9-bbfe-71e4efeeb9e1"));

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ManagingShopId",
                table: "Accounts");

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountStatusId", "AddressLine", "Birthday", "CreatedDate", "Email", "Gender", "ModifiedDate", "Name", "Password", "Phone", "WardId", "WorkingShopId" },
                values: new object[] { new Guid("a4247434-cb54-487e-ba75-42036402a949"), 2, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@camai.com", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "9eb622419ace52f259e858a7f2a10743d35e36fe0d22fc2d224c320cbc68d3af", null, null, null });

            migrationBuilder.InsertData(
                table: "AccountRole",
                columns: new[] { "AccountId", "RoleId" },
                values: new object[] { new Guid("a4247434-cb54-487e-ba75-42036402a949"), 1 });
        }
    }
}
