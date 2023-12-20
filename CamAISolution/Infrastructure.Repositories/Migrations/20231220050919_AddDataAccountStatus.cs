using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddDataAccountStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountRole",
                keyColumns: new[] { "AccountId", "RoleId" },
                keyValues: new object[] { new Guid("32915eea-c925-40c0-951b-f2e47605d738"), new Guid("264d8150-8ca6-4053-aa86-ffa7f48b859d") });

            migrationBuilder.DeleteData(
                table: "AccountStatuses",
                keyColumn: "Id",
                keyValue: new Guid("57b3d470-6e56-42ea-86a2-1829d7ca36f3"));

            migrationBuilder.DeleteData(
                table: "BrandStatuses",
                keyColumn: "Id",
                keyValue: new Guid("23f91444-f222-415e-a157-0b3025484790"));

            migrationBuilder.DeleteData(
                table: "BrandStatuses",
                keyColumn: "Id",
                keyValue: new Guid("d9cf4ff4-feea-497d-8b16-aecbe79313b5"));

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: new Guid("cf4becd3-a873-4442-8125-1aded90ce9fe"));

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: new Guid("da289168-d11a-4bee-a7ee-0ffdac975da4"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("38b513f0-6259-4662-a77a-f1c11fceb6b0"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("3e240936-d048-4c21-87fb-efd7607b68b6"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7f1c2c74-1d31-4b4f-a68c-07bafec0b531"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a891d4b6-57a5-4456-8fae-1d7d39882815"));

            migrationBuilder.DeleteData(
                table: "ShopStatuses",
                keyColumn: "Id",
                keyValue: new Guid("09766616-57b4-4696-9fa8-a2178c93f3f2"));

            migrationBuilder.DeleteData(
                table: "ShopStatuses",
                keyColumn: "Id",
                keyValue: new Guid("866f5ce5-c8d3-4fb6-82f8-ffd6ef368895"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("32915eea-c925-40c0-951b-f2e47605d738"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("264d8150-8ca6-4053-aa86-ffa7f48b859d"));

            migrationBuilder.DeleteData(
                table: "AccountStatuses",
                keyColumn: "Id",
                keyValue: new Guid("10c2d167-a1ff-49c7-bf3a-67036a4fc2b6"));

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "AccountStatuses",
                columns: new[] { "Id", "CreatedDate", "Description", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("10c2d167-a1ff-49c7-bf3a-67036a4fc2b6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { new Guid("57b3d470-6e56-42ea-86a2-1829d7ca36f3"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inactive" }
                });

            migrationBuilder.InsertData(
                table: "BrandStatuses",
                columns: new[] { "Id", "CreatedDate", "Description", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("23f91444-f222-415e-a157-0b3025484790"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { new Guid("d9cf4ff4-feea-497d-8b16-aecbe79313b5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inactive" }
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "CreatedDate", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("cf4becd3-a873-4442-8125-1aded90ce9fe"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male" },
                    { new Guid("da289168-d11a-4bee-a7ee-0ffdac975da4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Female" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "Description", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("264d8150-8ca6-4053-aa86-ffa7f48b859d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin" },
                    { new Guid("38b513f0-6259-4662-a77a-f1c11fceb6b0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shop manager" },
                    { new Guid("3e240936-d048-4c21-87fb-efd7607b68b6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Technician" },
                    { new Guid("7f1c2c74-1d31-4b4f-a68c-07bafec0b531"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Brand manager" },
                    { new Guid("a891d4b6-57a5-4456-8fae-1d7d39882815"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Employee" }
                });

            migrationBuilder.InsertData(
                table: "ShopStatuses",
                columns: new[] { "Id", "CreatedDate", "Description", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("09766616-57b4-4696-9fa8-a2178c93f3f2"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inactive" },
                    { new Guid("866f5ce5-c8d3-4fb6-82f8-ffd6ef368895"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountStatusId", "AddressLine", "Birthday", "CreatedDate", "Email", "GenderId", "ModifiedDate", "Name", "Password", "Phone", "WardId", "WorkingShopId" },
                values: new object[] { new Guid("32915eea-c925-40c0-951b-f2e47605d738"), new Guid("10c2d167-a1ff-49c7-bf3a-67036a4fc2b6"), null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@camai.com", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "9eb622419ace52f259e858a7f2a10743d35e36fe0d22fc2d224c320cbc68d3af", null, null, null });

            migrationBuilder.InsertData(
                table: "AccountRole",
                columns: new[] { "AccountId", "RoleId" },
                values: new object[] { new Guid("32915eea-c925-40c0-951b-f2e47605d738"), new Guid("264d8150-8ca6-4053-aa86-ffa7f48b859d") });
        }
    }
}
