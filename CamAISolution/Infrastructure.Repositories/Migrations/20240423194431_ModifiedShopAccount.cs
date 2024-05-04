using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedShopAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Shops",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shops_ExternalId",
                table: "Shops",
                column: "ExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ExternalId",
                table: "Accounts",
                column: "ExternalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Shops_ExternalId",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_ExternalId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Accounts");
        }
    }
}
