using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountRole",
                keyColumns: new[] { "AccountId", "RoleId" },
                keyValues: new object[] { new Guid("4e0f625a-b000-4d9a-9ecb-82f37c8950f5"), 1 });

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("4e0f625a-b000-4d9a-9ecb-82f37c8950f5"));

            migrationBuilder.CreateTable(
                name: "RequestStatuses",
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
                    table.PrimaryKey("PK_RequestStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestTypes",
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
                    table.PrimaryKey("PK_RequestTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketStatuses",
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
                    table.PrimaryKey("PK_TicketStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketTypes",
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
                    table.PrimaryKey("PK_TicketTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestTypeId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reply = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestStatusId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        name: "FK_Requests_RequestStatuses_RequestStatusId",
                        column: x => x.RequestStatusId,
                        principalTable: "RequestStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_RequestTypes_RequestTypeId",
                        column: x => x.RequestTypeId,
                        principalTable: "RequestTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketTypeId = table.Column<int>(type: "int", nullable: false),
                    AssignedToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reply = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TicketStatusId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Accounts_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_TicketStatuses_TicketStatusId",
                        column: x => x.TicketStatusId,
                        principalTable: "TicketStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketTypes_TicketTypeId",
                        column: x => x.TicketTypeId,
                        principalTable: "TicketTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldStatusId = table.Column<int>(type: "int", nullable: true),
                    NewStatusId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestActivities_Accounts_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestActivities_RequestStatuses_NewStatusId",
                        column: x => x.NewStatusId,
                        principalTable: "RequestStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RequestActivities_RequestStatuses_OldStatusId",
                        column: x => x.OldStatusId,
                        principalTable: "RequestStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RequestActivities_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TicketActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OldStatusId = table.Column<int>(type: "int", nullable: true),
                    NewStatusId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketActivities_Accounts_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketActivities_TicketStatuses_NewStatusId",
                        column: x => x.NewStatusId,
                        principalTable: "TicketStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketActivities_TicketStatuses_OldStatusId",
                        column: x => x.OldStatusId,
                        principalTable: "TicketStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketActivities_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountStatusId", "AddressLine", "Birthday", "CreatedDate", "Email", "Gender", "ModifiedDate", "Name", "Password", "Phone", "WardId", "WorkingShopId" },
                values: new object[] { new Guid("bda4d1d9-0021-461d-b6a2-e530f633ee23"), 2, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@camai.com", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "9eb622419ace52f259e858a7f2a10743d35e36fe0d22fc2d224c320cbc68d3af", null, null, null });

            migrationBuilder.InsertData(
                table: "RequestStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Open" },
                    { 2, null, "Cancelled" },
                    { 3, null, "Done" },
                    { 4, null, "Rejected" }
                });

            migrationBuilder.InsertData(
                table: "RequestTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Install" },
                    { 2, null, "Repair" },
                    { 3, null, "Remove" }
                });

            migrationBuilder.InsertData(
                table: "TicketStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Open" },
                    { 2, null, "Cancelled" },
                    { 3, null, "Done" },
                    { 4, null, "Failed" }
                });

            migrationBuilder.InsertData(
                table: "TicketTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Install" },
                    { 2, null, "Repair" },
                    { 3, null, "Remove" }
                });

            migrationBuilder.InsertData(
                table: "AccountRole",
                columns: new[] { "AccountId", "RoleId" },
                values: new object[] { new Guid("bda4d1d9-0021-461d-b6a2-e530f633ee23"), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_RequestActivities_ModifiedById",
                table: "RequestActivities",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_RequestActivities_NewStatusId",
                table: "RequestActivities",
                column: "NewStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestActivities_OldStatusId",
                table: "RequestActivities",
                column: "OldStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestActivities_RequestId",
                table: "RequestActivities",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_AccountId",
                table: "Requests",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestStatusId",
                table: "Requests",
                column: "RequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestTypeId",
                table: "Requests",
                column: "RequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ShopId",
                table: "Requests",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketActivities_ModifiedById",
                table: "TicketActivities",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_TicketActivities_NewStatusId",
                table: "TicketActivities",
                column: "NewStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketActivities_OldStatusId",
                table: "TicketActivities",
                column: "OldStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketActivities_TicketId",
                table: "TicketActivities",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssignedToId",
                table: "Tickets",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ShopId",
                table: "Tickets",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketStatusId",
                table: "Tickets",
                column: "TicketStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketTypeId",
                table: "Tickets",
                column: "TicketTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestActivities");

            migrationBuilder.DropTable(
                name: "TicketActivities");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "RequestStatuses");

            migrationBuilder.DropTable(
                name: "RequestTypes");

            migrationBuilder.DropTable(
                name: "TicketStatuses");

            migrationBuilder.DropTable(
                name: "TicketTypes");

            migrationBuilder.DeleteData(
                table: "AccountRole",
                keyColumns: new[] { "AccountId", "RoleId" },
                keyValues: new object[] { new Guid("bda4d1d9-0021-461d-b6a2-e530f633ee23"), 1 });

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("bda4d1d9-0021-461d-b6a2-e530f633ee23"));

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountStatusId", "AddressLine", "Birthday", "CreatedDate", "Email", "Gender", "ModifiedDate", "Name", "Password", "Phone", "WardId", "WorkingShopId" },
                values: new object[] { new Guid("4e0f625a-b000-4d9a-9ecb-82f37c8950f5"), 2, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@camai.com", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "9eb622419ace52f259e858a7f2a10743d35e36fe0d22fc2d224c320cbc68d3af", null, null, null });

            migrationBuilder.InsertData(
                table: "AccountRole",
                columns: new[] { "AccountId", "RoleId" },
                values: new object[] { new Guid("4e0f625a-b000-4d9a-9ecb-82f37c8950f5"), 1 });
        }
    }
}
