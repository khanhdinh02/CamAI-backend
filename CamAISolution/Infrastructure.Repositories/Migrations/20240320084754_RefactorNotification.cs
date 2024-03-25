using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RefactorNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Accounts_SentById",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_SentById",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SentById",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "NotificationType",
                table: "Notifications",
                newName: "Type");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "RelatedEntityId",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AccountId",
                table: "Notifications",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Accounts_AccountId",
                table: "Notifications",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Accounts_AccountId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_AccountId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "RelatedEntityId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Notifications",
                newName: "NotificationType");

            migrationBuilder.AddColumn<Guid>(
                name: "SentById",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SentById",
                table: "Notifications",
                column: "SentById");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Accounts_SentById",
                table: "Notifications",
                column: "SentById",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
