using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class EditBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Behaviors_Accounts_AccountId",
                table: "Behaviors");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Behaviors",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Behaviors_AccountId",
                table: "Behaviors",
                newName: "IX_Behaviors_EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Behaviors_Employees_EmployeeId",
                table: "Behaviors",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Behaviors_Employees_EmployeeId",
                table: "Behaviors");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Behaviors",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Behaviors_EmployeeId",
                table: "Behaviors",
                newName: "IX_Behaviors_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Behaviors_Accounts_AccountId",
                table: "Behaviors",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
