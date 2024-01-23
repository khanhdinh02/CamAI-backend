using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddBrandManagerField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BrandManagerId",
                table: "Brands",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_BrandManagerId",
                table: "Brands",
                column: "BrandManagerId",
                unique: true,
                filter: "[BrandManagerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Accounts_BrandManagerId",
                table: "Brands",
                column: "BrandManagerId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Accounts_BrandManagerId",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Brands_BrandManagerId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "BrandManagerId",
                table: "Brands");
        }
    }
}
