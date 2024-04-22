using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddBrandDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrandWebsite",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyAddress",
                table: "Brands",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Brands",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CompanyWardId",
                table: "Brands",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_CompanyWardId",
                table: "Brands",
                column: "CompanyWardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Wards_CompanyWardId",
                table: "Brands",
                column: "CompanyWardId",
                principalTable: "Wards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Wards_CompanyWardId",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Brands_CompanyWardId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "BrandWebsite",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "CompanyAddress",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "CompanyWardId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Brands");
        }
    }
}
