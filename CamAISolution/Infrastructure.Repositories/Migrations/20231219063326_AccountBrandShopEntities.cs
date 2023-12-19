using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AccountBrandShopEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Shops",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Shops",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Shops",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "WardId",
                table: "Shops",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "AddressLine",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Birthday",
                table: "Accounts",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "GenderId",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Accounts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Accounts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShopId",
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

            migrationBuilder.AddColumn<Guid>(
                name: "WardId",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountRole",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRole", x => new { x.AccountId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AccountRole_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_Shops_WardId",
                table: "Shops",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_BrandId",
                table: "Accounts",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_GenderId",
                table: "Accounts",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ShopId",
                table: "Accounts",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_WardId",
                table: "Accounts",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRole_RoleId",
                table: "AccountRole",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Brands_BrandId",
                table: "Accounts",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Genders_GenderId",
                table: "Accounts",
                column: "GenderId",
                principalTable: "Genders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Shops_ShopId",
                table: "Accounts",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Wards_WardId",
                table: "Accounts",
                column: "WardId",
                principalTable: "Wards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Wards_WardId",
                table: "Shops",
                column: "WardId",
                principalTable: "Wards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Brands_BrandId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Genders_GenderId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Shops_ShopId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Wards_WardId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Wards_WardId",
                table: "Shops");

            migrationBuilder.DropTable(
                name: "AccountRole");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Shops_WardId",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_BrandId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_GenderId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_ShopId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_WardId",
                table: "Accounts");

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("49a589fc-c5be-4f75-b48a-7f6408ea46ba"));

            migrationBuilder.DropColumn(
                name: "AddressLine",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "WardId",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "AddressLine",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "GenderId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "WardId",
                table: "Accounts");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
