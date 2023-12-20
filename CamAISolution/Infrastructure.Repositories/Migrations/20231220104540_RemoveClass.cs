using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RemoveClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountRole",
                table: "AccountRole");

            migrationBuilder.DropIndex(
                name: "IX_AccountRole_RoleId",
                table: "AccountRole");

            migrationBuilder.DeleteData(
                table: "AccountRole",
                keyColumns: new[] { "AccountId", "RoleId" },
                keyValues: new object[] { new Guid("6a29f2b2-8383-400f-b2e4-55ed0d73baf2"), new Guid("445e1e75-a049-4080-b7b9-39f0230db10b") });

            migrationBuilder.DeleteData(
                table: "AccountStatuses",
                keyColumn: "Id",
                keyValue: new Guid("7fe2231d-eff1-49a5-83a0-08b3d24e23c1"));

            migrationBuilder.DeleteData(
                table: "AccountStatuses",
                keyColumn: "Id",
                keyValue: new Guid("d6b279cf-9db7-48b4-a757-0e794721ffba"));

            migrationBuilder.DeleteData(
                table: "BrandStatuses",
                keyColumn: "Id",
                keyValue: new Guid("db3cd2ca-46a9-4ba6-b45f-495399fbe499"));

            migrationBuilder.DeleteData(
                table: "BrandStatuses",
                keyColumn: "Id",
                keyValue: new Guid("fd8b6d47-8022-4c09-812f-b54057bd4609"));

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: new Guid("852936da-122b-4f14-9431-36968d57a30b"));

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: new Guid("ecc62be4-c48f-4d58-bfd5-14e4317352a3"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("10fc3a85-cb58-45a1-8f6e-e97859ea6a07"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("1f8005b4-a7e0-4829-94b0-55d58e79200f"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("3950dc6f-d89b-42a1-b6de-cc14ec3755ef"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("e7e269b9-a273-4212-96f3-063c22cb513e"));

            migrationBuilder.DeleteData(
                table: "ShopStatuses",
                keyColumn: "Id",
                keyValue: new Guid("7e0ef477-5f32-498b-b1e6-5cb5a79192a4"));

            migrationBuilder.DeleteData(
                table: "ShopStatuses",
                keyColumn: "Id",
                keyValue: new Guid("d49dd0b3-a5c6-4b16-9527-2674578fc05b"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("6a29f2b2-8383-400f-b2e4-55ed0d73baf2"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("445e1e75-a049-4080-b7b9-39f0230db10b"));

            migrationBuilder.DeleteData(
                table: "AccountStatuses",
                keyColumn: "Id",
                keyValue: new Guid("b9a32e8b-b87e-4d30-8b7e-2d0d95ba25d4"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountRole",
                table: "AccountRole",
                columns: new[] { "RoleId", "AccountId" });

            migrationBuilder.InsertData(
                table: "AccountStatuses",
                columns: new[] { "Id", "CreatedDate", "Description", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("51b09cee-aaa6-4c59-8432-211f4b210e50"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { new Guid("d22d481b-d2ca-455b-87a8-fe98b47902c0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "New" },
                    { new Guid("d7f31ae9-b0ed-48cf-9475-adec4d6c29d8"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inactive" }
                });

            migrationBuilder.InsertData(
                table: "BrandStatuses",
                columns: new[] { "Id", "CreatedDate", "Description", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("14ac7824-3ff2-4fc0-ad3c-d16ab43ce491"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inactive" },
                    { new Guid("244c3192-2578-47cc-b5fe-ca8a412cf52f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" }
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "CreatedDate", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("187e617b-3fde-4d17-814e-b9eaca0772aa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male" },
                    { new Guid("c12b5875-1d48-413b-85e8-99fbb0f65d97"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Female" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "Description", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("0c0f22dc-a447-4fff-9d88-a6eb77e75d51"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Brand manager" },
                    { new Guid("100dfd86-56c5-4860-a729-b93d0bc77e74"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Technician" },
                    { new Guid("83432b63-1951-4ace-8e35-97904af3094d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shop manager" },
                    { new Guid("c1cf9d85-4e95-4892-9220-aa0672278c36"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin" },
                    { new Guid("e46312ee-3df3-47e6-8bc5-8dd4fac7ce51"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Employee" }
                });

            migrationBuilder.InsertData(
                table: "ShopStatuses",
                columns: new[] { "Id", "CreatedDate", "Description", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("134c84ad-9016-46ab-9462-4200b2a463f0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inactive" },
                    { new Guid("d9af567d-5460-4618-8673-95a83e6da1a7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountStatusId", "AddressLine", "Birthday", "CreatedDate", "Email", "GenderId", "ModifiedDate", "Name", "Password", "Phone", "WardId", "WorkingShopId" },
                values: new object[] { new Guid("40f471c0-a8ad-4215-972c-645e5cd30630"), new Guid("51b09cee-aaa6-4c59-8432-211f4b210e50"), null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@camai.com", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "9eb622419ace52f259e858a7f2a10743d35e36fe0d22fc2d224c320cbc68d3af", null, null, null });

            migrationBuilder.InsertData(
                table: "AccountRole",
                columns: new[] { "AccountId", "RoleId" },
                values: new object[] { new Guid("40f471c0-a8ad-4215-972c-645e5cd30630"), new Guid("c1cf9d85-4e95-4892-9220-aa0672278c36") });

            migrationBuilder.CreateIndex(
                name: "IX_AccountRole_AccountId",
                table: "AccountRole",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountRole",
                table: "AccountRole");

            migrationBuilder.DropIndex(
                name: "IX_AccountRole_AccountId",
                table: "AccountRole");

            migrationBuilder.DeleteData(
                table: "AccountRole",
                keyColumns: new[] { "AccountId", "RoleId" },
                keyValues: new object[] { new Guid("40f471c0-a8ad-4215-972c-645e5cd30630"), new Guid("c1cf9d85-4e95-4892-9220-aa0672278c36") });

            migrationBuilder.DeleteData(
                table: "AccountStatuses",
                keyColumn: "Id",
                keyValue: new Guid("d22d481b-d2ca-455b-87a8-fe98b47902c0"));

            migrationBuilder.DeleteData(
                table: "AccountStatuses",
                keyColumn: "Id",
                keyValue: new Guid("d7f31ae9-b0ed-48cf-9475-adec4d6c29d8"));

            migrationBuilder.DeleteData(
                table: "BrandStatuses",
                keyColumn: "Id",
                keyValue: new Guid("14ac7824-3ff2-4fc0-ad3c-d16ab43ce491"));

            migrationBuilder.DeleteData(
                table: "BrandStatuses",
                keyColumn: "Id",
                keyValue: new Guid("244c3192-2578-47cc-b5fe-ca8a412cf52f"));

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: new Guid("187e617b-3fde-4d17-814e-b9eaca0772aa"));

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: new Guid("c12b5875-1d48-413b-85e8-99fbb0f65d97"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0c0f22dc-a447-4fff-9d88-a6eb77e75d51"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("100dfd86-56c5-4860-a729-b93d0bc77e74"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("83432b63-1951-4ace-8e35-97904af3094d"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("e46312ee-3df3-47e6-8bc5-8dd4fac7ce51"));

            migrationBuilder.DeleteData(
                table: "ShopStatuses",
                keyColumn: "Id",
                keyValue: new Guid("134c84ad-9016-46ab-9462-4200b2a463f0"));

            migrationBuilder.DeleteData(
                table: "ShopStatuses",
                keyColumn: "Id",
                keyValue: new Guid("d9af567d-5460-4618-8673-95a83e6da1a7"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("40f471c0-a8ad-4215-972c-645e5cd30630"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c1cf9d85-4e95-4892-9220-aa0672278c36"));

            migrationBuilder.DeleteData(
                table: "AccountStatuses",
                keyColumn: "Id",
                keyValue: new Guid("51b09cee-aaa6-4c59-8432-211f4b210e50"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountRole",
                table: "AccountRole",
                columns: new[] { "AccountId", "RoleId" });

            migrationBuilder.InsertData(
                table: "AccountStatuses",
                columns: new[] { "Id", "CreatedDate", "Description", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("7fe2231d-eff1-49a5-83a0-08b3d24e23c1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "New" },
                    { new Guid("b9a32e8b-b87e-4d30-8b7e-2d0d95ba25d4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { new Guid("d6b279cf-9db7-48b4-a757-0e794721ffba"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inactive" }
                });

            migrationBuilder.InsertData(
                table: "BrandStatuses",
                columns: new[] { "Id", "CreatedDate", "Description", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("db3cd2ca-46a9-4ba6-b45f-495399fbe499"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inactive" },
                    { new Guid("fd8b6d47-8022-4c09-812f-b54057bd4609"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" }
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "CreatedDate", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("852936da-122b-4f14-9431-36968d57a30b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male" },
                    { new Guid("ecc62be4-c48f-4d58-bfd5-14e4317352a3"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Female" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "Description", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("10fc3a85-cb58-45a1-8f6e-e97859ea6a07"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Brand manager" },
                    { new Guid("1f8005b4-a7e0-4829-94b0-55d58e79200f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Technician" },
                    { new Guid("3950dc6f-d89b-42a1-b6de-cc14ec3755ef"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shop manager" },
                    { new Guid("445e1e75-a049-4080-b7b9-39f0230db10b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin" },
                    { new Guid("e7e269b9-a273-4212-96f3-063c22cb513e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Employee" }
                });

            migrationBuilder.InsertData(
                table: "ShopStatuses",
                columns: new[] { "Id", "CreatedDate", "Description", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("7e0ef477-5f32-498b-b1e6-5cb5a79192a4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { new Guid("d49dd0b3-a5c6-4b16-9527-2674578fc05b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inactive" }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountStatusId", "AddressLine", "Birthday", "CreatedDate", "Email", "GenderId", "ModifiedDate", "Name", "Password", "Phone", "WardId", "WorkingShopId" },
                values: new object[] { new Guid("6a29f2b2-8383-400f-b2e4-55ed0d73baf2"), new Guid("b9a32e8b-b87e-4d30-8b7e-2d0d95ba25d4"), null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@camai.com", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "9eb622419ace52f259e858a7f2a10743d35e36fe0d22fc2d224c320cbc68d3af", null, null, null });

            migrationBuilder.InsertData(
                table: "AccountRole",
                columns: new[] { "AccountId", "RoleId" },
                values: new object[] { new Guid("6a29f2b2-8383-400f-b2e4-55ed0d73baf2"), new Guid("445e1e75-a049-4080-b7b9-39f0230db10b") });

            migrationBuilder.CreateIndex(
                name: "IX_AccountRole_RoleId",
                table: "AccountRole",
                column: "RoleId");
        }
    }
}
