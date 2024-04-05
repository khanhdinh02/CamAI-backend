using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEdgeBoxInstallActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EdgeBoxActivities_Accounts_ModifiedById",
                table: "EdgeBoxActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_EdgeBoxActivities_EdgeBoxes_EdgeBoxId",
                table: "EdgeBoxActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestActivities_Accounts_ModifiedById",
                table: "RequestActivities");

            migrationBuilder.DropTable(
                name: "EdgeBoxInstallActivities");

            migrationBuilder.DropColumn(
                name: "NewStatus",
                table: "EdgeBoxActivities");

            migrationBuilder.DropColumn(
                name: "OldStatus",
                table: "EdgeBoxActivities");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedById",
                table: "RequestActivities",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedById",
                table: "EdgeBoxActivities",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "EdgeBoxId",
                table: "EdgeBoxActivities",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "EdgeBoxInstallId",
                table: "EdgeBoxActivities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "EdgeBoxActivities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EdgeBoxActivities_EdgeBoxInstallId",
                table: "EdgeBoxActivities",
                column: "EdgeBoxInstallId");

            migrationBuilder.AddForeignKey(
                name: "FK_EdgeBoxActivities_Accounts_ModifiedById",
                table: "EdgeBoxActivities",
                column: "ModifiedById",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EdgeBoxActivities_EdgeBoxInstalls_EdgeBoxInstallId",
                table: "EdgeBoxActivities",
                column: "EdgeBoxInstallId",
                principalTable: "EdgeBoxInstalls",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EdgeBoxActivities_EdgeBoxes_EdgeBoxId",
                table: "EdgeBoxActivities",
                column: "EdgeBoxId",
                principalTable: "EdgeBoxes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestActivities_Accounts_ModifiedById",
                table: "RequestActivities",
                column: "ModifiedById",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EdgeBoxActivities_Accounts_ModifiedById",
                table: "EdgeBoxActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_EdgeBoxActivities_EdgeBoxInstalls_EdgeBoxInstallId",
                table: "EdgeBoxActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_EdgeBoxActivities_EdgeBoxes_EdgeBoxId",
                table: "EdgeBoxActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestActivities_Accounts_ModifiedById",
                table: "RequestActivities");

            migrationBuilder.DropIndex(
                name: "IX_EdgeBoxActivities_EdgeBoxInstallId",
                table: "EdgeBoxActivities");

            migrationBuilder.DropColumn(
                name: "EdgeBoxInstallId",
                table: "EdgeBoxActivities");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "EdgeBoxActivities");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedById",
                table: "RequestActivities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedById",
                table: "EdgeBoxActivities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EdgeBoxId",
                table: "EdgeBoxActivities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewStatus",
                table: "EdgeBoxActivities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OldStatus",
                table: "EdgeBoxActivities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EdgeBoxInstallActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EdgeBoxInstallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NewStatus = table.Column<int>(type: "int", nullable: false),
                    OldStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdgeBoxInstallActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EdgeBoxInstallActivities_Accounts_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EdgeBoxInstallActivities_EdgeBoxInstalls_EdgeBoxInstallId",
                        column: x => x.EdgeBoxInstallId,
                        principalTable: "EdgeBoxInstalls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EdgeBoxInstallActivities_EdgeBoxInstallId",
                table: "EdgeBoxInstallActivities",
                column: "EdgeBoxInstallId");

            migrationBuilder.CreateIndex(
                name: "IX_EdgeBoxInstallActivities_ModifiedById",
                table: "EdgeBoxInstallActivities",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_EdgeBoxActivities_Accounts_ModifiedById",
                table: "EdgeBoxActivities",
                column: "ModifiedById",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EdgeBoxActivities_EdgeBoxes_EdgeBoxId",
                table: "EdgeBoxActivities",
                column: "EdgeBoxId",
                principalTable: "EdgeBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestActivities_Accounts_ModifiedById",
                table: "RequestActivities",
                column: "ModifiedById",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
