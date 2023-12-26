using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountRole",
                keyColumns: new[] { "AccountId", "RoleId" },
                keyValues: new object[] { new Guid("bda4d1d9-0021-461d-b6a2-e530f633ee23"), 1 });

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("bda4d1d9-0021-461d-b6a2-e530f633ee23"));

            migrationBuilder.CreateTable(
                name: "BehaviorTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehaviorTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EvidenceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvidenceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Behaviors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BehaviorTypeId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CameraId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Behaviors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Behaviors_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Behaviors_BehaviorTypes_BehaviorTypeId",
                        column: x => x.BehaviorTypeId,
                        principalTable: "BehaviorTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Behaviors_Cameras_CameraId",
                        column: x => x.CameraId,
                        principalTable: "Cameras",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Evidences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvidenceTypeId = table.Column<int>(type: "int", nullable: false),
                    BehaviorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evidences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evidences_Behaviors_BehaviorId",
                        column: x => x.BehaviorId,
                        principalTable: "Behaviors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Evidences_EvidenceTypes_EvidenceTypeId",
                        column: x => x.EvidenceTypeId,
                        principalTable: "EvidenceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountStatusId", "AddressLine", "Birthday", "CreatedDate", "Email", "Gender", "ModifiedDate", "Name", "Password", "Phone", "WardId", "WorkingShopId" },
                values: new object[] { new Guid("b688445c-82d9-43b8-8e84-eaa1c2522adf"), 2, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@camai.com", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "9eb622419ace52f259e858a7f2a10743d35e36fe0d22fc2d224c320cbc68d3af", null, null, null });

            migrationBuilder.InsertData(
                table: "AccountRole",
                columns: new[] { "AccountId", "RoleId" },
                values: new object[] { new Guid("b688445c-82d9-43b8-8e84-eaa1c2522adf"), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Behaviors_AccountId",
                table: "Behaviors",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Behaviors_BehaviorTypeId",
                table: "Behaviors",
                column: "BehaviorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Behaviors_CameraId",
                table: "Behaviors",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_BehaviorId",
                table: "Evidences",
                column: "BehaviorId");

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_EvidenceTypeId",
                table: "Evidences",
                column: "EvidenceTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Evidences");

            migrationBuilder.DropTable(
                name: "Behaviors");

            migrationBuilder.DropTable(
                name: "EvidenceTypes");

            migrationBuilder.DropTable(
                name: "BehaviorTypes");

            migrationBuilder.DeleteData(
                table: "AccountRole",
                keyColumns: new[] { "AccountId", "RoleId" },
                keyValues: new object[] { new Guid("b688445c-82d9-43b8-8e84-eaa1c2522adf"), 1 });

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("b688445c-82d9-43b8-8e84-eaa1c2522adf"));

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountStatusId", "AddressLine", "Birthday", "CreatedDate", "Email", "Gender", "ModifiedDate", "Name", "Password", "Phone", "WardId", "WorkingShopId" },
                values: new object[] { new Guid("bda4d1d9-0021-461d-b6a2-e530f633ee23"), 2, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@camai.com", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "9eb622419ace52f259e858a7f2a10743d35e36fe0d22fc2d224c320cbc68d3af", null, null, null });

            migrationBuilder.InsertData(
                table: "AccountRole",
                columns: new[] { "AccountId", "RoleId" },
                values: new object[] { new Guid("bda4d1d9-0021-461d-b6a2-e530f633ee23"), 1 });
        }
    }
}
