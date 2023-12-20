using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Brands_BrandId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Shops_ShopId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_BrandId",
                table: "Accounts");

            migrationBuilder.DeleteData(
                table: "AccountRole",
                keyColumns: new[] { "AccountId", "RoleId" },
                keyValues: new object[] { new Guid("49a589fc-c5be-4f75-b48a-7f6408ea46ba"), new Guid("4b17a852-9448-49b6-9e2a-f8722836574e") });

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: new Guid("4e33638d-4580-4b7c-adfe-f788694f7dd3"));

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: new Guid("c48e345a-88a9-488b-9646-f2b1f418d389"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("8b09965b-e19f-4f17-aa8f-63a9350dfcf4"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c1753259-efa3-4fa6-bb8b-126f5cb19f3e"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("ccfe6a28-cf35-4821-a220-6d0c7e51f8f9"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("dd454529-580a-4889-bcb7-ac0c819725e3"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("49a589fc-c5be-4f75-b48a-7f6408ea46ba"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("4b17a852-9448-49b6-9e2a-f8722836574e"));

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "ShopId",
                table: "Accounts",
                newName: "WorkingShopId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_ShopId",
                table: "Accounts",
                newName: "IX_Accounts_WorkingShopId");

            migrationBuilder.AddColumn<Guid>(
                name: "ShopManagerId",
                table: "Shops",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShopStatusId",
                table: "Shops",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BrandManagerId",
                table: "Brands",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BrandStatusId",
                table: "Brands",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AccountStatusId",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AccountStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrandStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopStatuses", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Shops_ShopManagerId",
                table: "Shops",
                column: "ShopManagerId",
                unique: true,
                filter: "[ShopManagerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_ShopStatusId",
                table: "Shops",
                column: "ShopStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_BrandManagerId",
                table: "Brands",
                column: "BrandManagerId",
                unique: true,
                filter: "[BrandManagerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_BrandStatusId",
                table: "Brands",
                column: "BrandStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountStatusId",
                table: "Accounts",
                column: "AccountStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_AccountStatuses_AccountStatusId",
                table: "Accounts",
                column: "AccountStatusId",
                principalTable: "AccountStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Shops_WorkingShopId",
                table: "Accounts",
                column: "WorkingShopId",
                principalTable: "Shops",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Accounts_BrandManagerId",
                table: "Brands",
                column: "BrandManagerId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_BrandStatuses_BrandStatusId",
                table: "Brands",
                column: "BrandStatusId",
                principalTable: "BrandStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Accounts_ShopManagerId",
                table: "Shops",
                column: "ShopManagerId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_ShopStatuses_ShopStatusId",
                table: "Shops",
                column: "ShopStatusId",
                principalTable: "ShopStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_AccountStatuses_AccountStatusId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Shops_WorkingShopId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Accounts_BrandManagerId",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Brands_BrandStatuses_BrandStatusId",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Accounts_ShopManagerId",
                table: "Shops");

            migrationBuilder.DropForeignKey(
                name: "FK_Shops_ShopStatuses_ShopStatusId",
                table: "Shops");

            migrationBuilder.DropTable(
                name: "AccountStatuses");

            migrationBuilder.DropTable(
                name: "BrandStatuses");

            migrationBuilder.DropTable(
                name: "ShopStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Shops_ShopManagerId",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Shops_ShopStatusId",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Brands_BrandManagerId",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Brands_BrandStatusId",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_AccountStatusId",
                table: "Accounts");

            migrationBuilder.DeleteData(
                table: "AccountRole",
                keyColumns: new[] { "AccountId", "RoleId" },
                keyValues: new object[] { new Guid("32915eea-c925-40c0-951b-f2e47605d738"), new Guid("264d8150-8ca6-4053-aa86-ffa7f48b859d") });

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
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("32915eea-c925-40c0-951b-f2e47605d738"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("264d8150-8ca6-4053-aa86-ffa7f48b859d"));

            migrationBuilder.DropColumn(
                name: "ShopManagerId",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "ShopStatusId",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "BrandManagerId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "BrandStatusId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "AccountStatusId",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "WorkingShopId",
                table: "Accounts",
                newName: "ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_WorkingShopId",
                table: "Accounts",
                newName: "IX_Accounts_ShopId");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Shops",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Brands",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Accounts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AddressLine", "Birthday", "BrandId", "CreatedDate", "Email", "GenderId", "ModifiedDate", "Name", "Password", "Phone", "ShopId", "Status", "WardId" },
                values: new object[] { new Guid("49a589fc-c5be-4f75-b48a-7f6408ea46ba"), null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@camai.com", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "9eb622419ace52f259e858a7f2a10743d35e36fe0d22fc2d224c320cbc68d3af", null, null, "Active", null });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "CreatedDate", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("4e33638d-4580-4b7c-adfe-f788694f7dd3"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Female" },
                    { new Guid("c48e345a-88a9-488b-9646-f2b1f418d389"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "Description", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("4b17a852-9448-49b6-9e2a-f8722836574e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin" },
                    { new Guid("8b09965b-e19f-4f17-aa8f-63a9350dfcf4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Brand manager" },
                    { new Guid("c1753259-efa3-4fa6-bb8b-126f5cb19f3e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shop manager" },
                    { new Guid("ccfe6a28-cf35-4821-a220-6d0c7e51f8f9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Technician" },
                    { new Guid("dd454529-580a-4889-bcb7-ac0c819725e3"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Employee" }
                });

            migrationBuilder.InsertData(
                table: "AccountRole",
                columns: new[] { "AccountId", "RoleId" },
                values: new object[] { new Guid("49a589fc-c5be-4f75-b48a-7f6408ea46ba"), new Guid("4b17a852-9448-49b6-9e2a-f8722836574e") });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_BrandId",
                table: "Accounts",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Brands_BrandId",
                table: "Accounts",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Shops_ShopId",
                table: "Accounts",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id");
        }
    }
}
