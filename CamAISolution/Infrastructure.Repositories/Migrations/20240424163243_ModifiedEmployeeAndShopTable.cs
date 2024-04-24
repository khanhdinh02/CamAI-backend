using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedEmployeeAndShopTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Wards_WardId",
                table: "Shops");

            migrationBuilder.AlterColumn<int>(
                name: "WardId",
                table: "Shops",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Employees",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ExternalId",
                table: "Employees",
                column: "ExternalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Wards_WardId",
                table: "Shops",
                column: "WardId",
                principalTable: "Wards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Wards_WardId",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ExternalId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "WardId",
                table: "Shops",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Wards_WardId",
                table: "Shops",
                column: "WardId",
                principalTable: "Wards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
