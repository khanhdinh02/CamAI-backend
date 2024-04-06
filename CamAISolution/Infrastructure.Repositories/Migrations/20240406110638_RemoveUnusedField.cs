using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestActivities");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "EdgeBoxes");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "EdgeBoxes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "EdgeBoxes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "EdgeBoxes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EdgeBoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reply = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestStatus = table.Column<int>(type: "int", nullable: false),
                    RequestType = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_EdgeBoxes_EdgeBoxId",
                        column: x => x.EdgeBoxId,
                        principalTable: "EdgeBoxes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requests_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RequestActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NewStatus = table.Column<int>(type: "int", nullable: true),
                    OldStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestActivities_Accounts_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RequestActivities_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestActivities_ModifiedById",
                table: "RequestActivities",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_RequestActivities_RequestId",
                table: "RequestActivities",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_AccountId",
                table: "Requests",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_EdgeBoxId",
                table: "Requests",
                column: "EdgeBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ShopId",
                table: "Requests",
                column: "ShopId");
        }
    }
}
