using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RefactorDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountNotifications_NotificationStatuses_StatusId",
                table: "AccountNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_AccountStatuses_AccountStatusId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Behaviors_BehaviorTypes_BehaviorTypeId",
                table: "Behaviors");

            migrationBuilder.DropForeignKey(
                name: "FK_Brands_BrandStatuses_BrandStatusId",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_EdgeBoxActivities_EdgeBoxStatuses_NewStatusId",
                table: "EdgeBoxActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_EdgeBoxActivities_EdgeBoxStatuses_OldStatusId",
                table: "EdgeBoxActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_EdgeBoxes_EdgeBoxLocation_EdgeBoxLocationId",
                table: "EdgeBoxes");

            migrationBuilder.DropForeignKey(
                name: "FK_EdgeBoxes_EdgeBoxStatuses_EdgeBoxStatusId",
                table: "EdgeBoxes");

            migrationBuilder.DropForeignKey(
                name: "FK_EdgeBoxInstalls_EdgeBoxInstallStatuses_EdgeBoxInstallStatusId",
                table: "EdgeBoxInstalls");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeStatuses_EmployeeStatusId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Evidences_EvidenceTypes_EvidenceTypeId",
                table: "Evidences");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestActivities_RequestStatuses_NewStatusId",
                table: "RequestActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestActivities_RequestStatuses_OldStatusId",
                table: "RequestActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_RequestStatuses_RequestStatusId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_RequestTypes_RequestTypeId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Shops_ShopStatuses_ShopStatusId",
                table: "Shops");

            migrationBuilder.DropTable(
                name: "AccountRole");

            migrationBuilder.DropTable(
                name: "AccountStatuses");

            migrationBuilder.DropTable(
                name: "BehaviorTypes");

            migrationBuilder.DropTable(
                name: "BrandStatuses");

            migrationBuilder.DropTable(
                name: "EdgeBoxInstallStatuses");

            migrationBuilder.DropTable(
                name: "EdgeBoxLocation");

            migrationBuilder.DropTable(
                name: "EdgeBoxStatuses");

            migrationBuilder.DropTable(
                name: "EmployeeShift");

            migrationBuilder.DropTable(
                name: "EmployeeStatuses");

            migrationBuilder.DropTable(
                name: "EvidenceTypes");

            migrationBuilder.DropTable(
                name: "NotificationStatuses");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "RequestStatuses");

            migrationBuilder.DropTable(
                name: "RequestTypes");

            migrationBuilder.DropTable(
                name: "ShopStatuses");

            migrationBuilder.DropTable(
                name: "TicketActivities");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "TicketStatuses");

            migrationBuilder.DropTable(
                name: "TicketTypes");

            migrationBuilder.DropIndex(
                name: "IX_Shops_ShopStatusId",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Requests_RequestStatusId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_RequestTypeId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_RequestActivities_NewStatusId",
                table: "RequestActivities");

            migrationBuilder.DropIndex(
                name: "IX_RequestActivities_OldStatusId",
                table: "RequestActivities");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Evidences_EvidenceTypeId",
                table: "Evidences");

            migrationBuilder.DropIndex(
                name: "IX_Employees_EmployeeStatusId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_EdgeBoxInstalls_EdgeBoxInstallStatusId",
                table: "EdgeBoxInstalls");

            migrationBuilder.DropIndex(
                name: "IX_EdgeBoxes_EdgeBoxLocationId",
                table: "EdgeBoxes");

            migrationBuilder.DropIndex(
                name: "IX_EdgeBoxes_EdgeBoxStatusId",
                table: "EdgeBoxes");

            migrationBuilder.DropIndex(
                name: "IX_EdgeBoxActivities_NewStatusId",
                table: "EdgeBoxActivities");

            migrationBuilder.DropIndex(
                name: "IX_EdgeBoxActivities_OldStatusId",
                table: "EdgeBoxActivities");

            migrationBuilder.DropIndex(
                name: "IX_Brands_BrandStatusId",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Behaviors_BehaviorTypeId",
                table: "Behaviors");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_AccountStatusId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_AccountNotifications_StatusId",
                table: "AccountNotifications");

            migrationBuilder.RenameColumn(
                name: "ShopStatusId",
                table: "Shops",
                newName: "ShopStatus");

            migrationBuilder.RenameColumn(
                name: "RequestTypeId",
                table: "Requests",
                newName: "RequestType");

            migrationBuilder.RenameColumn(
                name: "RequestStatusId",
                table: "Requests",
                newName: "RequestStatus");

            migrationBuilder.RenameColumn(
                name: "OldStatusId",
                table: "RequestActivities",
                newName: "OldStatus");

            migrationBuilder.RenameColumn(
                name: "NewStatusId",
                table: "RequestActivities",
                newName: "NewStatus");

            migrationBuilder.RenameColumn(
                name: "NotificationTypeId",
                table: "Notifications",
                newName: "NotificationType");

            migrationBuilder.RenameColumn(
                name: "EmployeeStatusId",
                table: "Employees",
                newName: "EmployeeStatus");

            migrationBuilder.RenameColumn(
                name: "EdgeBoxInstallStatusId",
                table: "EdgeBoxInstalls",
                newName: "EdgeBoxInstallStatus");

            migrationBuilder.RenameColumn(
                name: "EdgeBoxStatusId",
                table: "EdgeBoxes",
                newName: "EdgeBoxStatus");

            migrationBuilder.RenameColumn(
                name: "EdgeBoxLocationId",
                table: "EdgeBoxes",
                newName: "EdgeBoxLocation");

            migrationBuilder.RenameColumn(
                name: "OldStatusId",
                table: "EdgeBoxActivities",
                newName: "OldStatus");

            migrationBuilder.RenameColumn(
                name: "NewStatusId",
                table: "EdgeBoxActivities",
                newName: "NewStatus");

            migrationBuilder.RenameColumn(
                name: "BrandStatusId",
                table: "Brands",
                newName: "BrandStatus");

            migrationBuilder.RenameColumn(
                name: "BehaviorTypeId",
                table: "Behaviors",
                newName: "IncidentType");

            migrationBuilder.RenameColumn(
                name: "AccountStatusId",
                table: "Accounts",
                newName: "AccountStatus");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "AccountNotifications",
                newName: "Status");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Notifications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(200)");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)");

            migrationBuilder.AddColumn<int>(
                name: "EvidenceType",
                table: "Evidences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "Employees",
                type: "int",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "Accounts",
                type: "int",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AccountRoles",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRoles", x => new { x.AccountId, x.Role });
                    table.ForeignKey(
                        name: "FK_AccountRoles_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountRoles");

            migrationBuilder.DropColumn(
                name: "EvidenceType",
                table: "Evidences");

            migrationBuilder.RenameColumn(
                name: "ShopStatus",
                table: "Shops",
                newName: "ShopStatusId");

            migrationBuilder.RenameColumn(
                name: "RequestType",
                table: "Requests",
                newName: "RequestTypeId");

            migrationBuilder.RenameColumn(
                name: "RequestStatus",
                table: "Requests",
                newName: "RequestStatusId");

            migrationBuilder.RenameColumn(
                name: "OldStatus",
                table: "RequestActivities",
                newName: "OldStatusId");

            migrationBuilder.RenameColumn(
                name: "NewStatus",
                table: "RequestActivities",
                newName: "NewStatusId");

            migrationBuilder.RenameColumn(
                name: "NotificationType",
                table: "Notifications",
                newName: "NotificationTypeId");

            migrationBuilder.RenameColumn(
                name: "EmployeeStatus",
                table: "Employees",
                newName: "EmployeeStatusId");

            migrationBuilder.RenameColumn(
                name: "EdgeBoxInstallStatus",
                table: "EdgeBoxInstalls",
                newName: "EdgeBoxInstallStatusId");

            migrationBuilder.RenameColumn(
                name: "EdgeBoxStatus",
                table: "EdgeBoxes",
                newName: "EdgeBoxStatusId");

            migrationBuilder.RenameColumn(
                name: "EdgeBoxLocation",
                table: "EdgeBoxes",
                newName: "EdgeBoxLocationId");

            migrationBuilder.RenameColumn(
                name: "OldStatus",
                table: "EdgeBoxActivities",
                newName: "OldStatusId");

            migrationBuilder.RenameColumn(
                name: "NewStatus",
                table: "EdgeBoxActivities",
                newName: "NewStatusId");

            migrationBuilder.RenameColumn(
                name: "BrandStatus",
                table: "Brands",
                newName: "BrandStatusId");

            migrationBuilder.RenameColumn(
                name: "IncidentType",
                table: "Behaviors",
                newName: "BehaviorTypeId");

            migrationBuilder.RenameColumn(
                name: "AccountStatus",
                table: "Accounts",
                newName: "AccountStatusId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "AccountNotifications",
                newName: "StatusId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Notifications",
                type: "NVARCHAR(200)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Notifications",
                type: "NVARCHAR(MAX)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Employees",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Accounts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AccountStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BehaviorTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehaviorTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrandStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EdgeBoxInstallStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdgeBoxInstallStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EdgeBoxLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdgeBoxLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EdgeBoxStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdgeBoxStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EvidenceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvidenceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
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
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shifts_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
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
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountRole",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRole", x => new { x.RoleId, x.AccountId });
                    table.ForeignKey(
                        name: "FK_AccountRole_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeShift",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeShift", x => new { x.EmployeeId, x.ShiftId });
                    table.ForeignKey(
                        name: "FK_EmployeeShift_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeShift_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TicketStatusId = table.Column<int>(type: "int", nullable: false),
                    TicketTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reply = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
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
                name: "TicketActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewStatusId = table.Column<int>(type: "int", nullable: true),
                    OldStatusId = table.Column<int>(type: "int", nullable: true),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                table: "AccountStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "New" },
                    { 2, null, "Active" },
                    { 3, null, "Inactive" }
                });

            migrationBuilder.InsertData(
                table: "BrandStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Active" },
                    { 2, null, "Inactive" }
                });

            migrationBuilder.InsertData(
                table: "EdgeBoxInstallStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Valid" },
                    { 2, null, "Expired" }
                });

            migrationBuilder.InsertData(
                table: "EdgeBoxLocation",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Idle" },
                    { 2, null, "Installing" },
                    { 3, null, "Occupied" },
                    { 4, null, "Uninstalling" },
                    { 5, null, "Disposed" }
                });

            migrationBuilder.InsertData(
                table: "EdgeBoxStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Active" },
                    { 2, null, "Inactive" },
                    { 3, null, "Broken" }
                });

            migrationBuilder.InsertData(
                table: "EmployeeStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Active" },
                    { 2, null, "Inactive" }
                });

            migrationBuilder.InsertData(
                table: "NotificationStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Unread" },
                    { 2, null, "Read" }
                });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Normal" },
                    { 2, null, "Warnning" },
                    { 3, null, "Urgent" }
                });

            migrationBuilder.InsertData(
                table: "RequestStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Open" },
                    { 2, null, "Canceled" },
                    { 3, null, "Done" },
                    { 4, null, "Rejected" }
                });

            migrationBuilder.InsertData(
                table: "RequestTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Install" },
                    { 3, null, "Repair" },
                    { 4, null, "Remove" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Admin" },
                    { 2, null, "Technician" },
                    { 3, null, "Brand manager" },
                    { 4, null, "Shop manager" },
                    { 5, null, "Employee" }
                });

            migrationBuilder.InsertData(
                table: "ShopStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Active" },
                    { 2, null, "Inactive" }
                });

            migrationBuilder.InsertData(
                table: "TicketStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "New" },
                    { 2, null, "Canceled" },
                    { 3, null, "Done" },
                    { 4, null, "Failed" },
                    { 5, null, "Active" }
                });

            migrationBuilder.InsertData(
                table: "TicketTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Install" },
                    { 3, null, "Repair" },
                    { 4, null, "Remove" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shops_ShopStatusId",
                table: "Shops",
                column: "ShopStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestStatusId",
                table: "Requests",
                column: "RequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestTypeId",
                table: "Requests",
                column: "RequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestActivities_NewStatusId",
                table: "RequestActivities",
                column: "NewStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestActivities_OldStatusId",
                table: "RequestActivities",
                column: "OldStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_EvidenceTypeId",
                table: "Evidences",
                column: "EvidenceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeStatusId",
                table: "Employees",
                column: "EmployeeStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EdgeBoxInstalls_EdgeBoxInstallStatusId",
                table: "EdgeBoxInstalls",
                column: "EdgeBoxInstallStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EdgeBoxes_EdgeBoxLocationId",
                table: "EdgeBoxes",
                column: "EdgeBoxLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_EdgeBoxes_EdgeBoxStatusId",
                table: "EdgeBoxes",
                column: "EdgeBoxStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EdgeBoxActivities_NewStatusId",
                table: "EdgeBoxActivities",
                column: "NewStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EdgeBoxActivities_OldStatusId",
                table: "EdgeBoxActivities",
                column: "OldStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_BrandStatusId",
                table: "Brands",
                column: "BrandStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Behaviors_BehaviorTypeId",
                table: "Behaviors",
                column: "BehaviorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountStatusId",
                table: "Accounts",
                column: "AccountStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountNotifications_StatusId",
                table: "AccountNotifications",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRole_AccountId",
                table: "AccountRole",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShift_ShiftId",
                table: "EmployeeShift",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_ShopId",
                table: "Shifts",
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

            migrationBuilder.AddForeignKey(
                name: "FK_AccountNotifications_NotificationStatuses_StatusId",
                table: "AccountNotifications",
                column: "StatusId",
                principalTable: "NotificationStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_AccountStatuses_AccountStatusId",
                table: "Accounts",
                column: "AccountStatusId",
                principalTable: "AccountStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Behaviors_BehaviorTypes_BehaviorTypeId",
                table: "Behaviors",
                column: "BehaviorTypeId",
                principalTable: "BehaviorTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_BrandStatuses_BrandStatusId",
                table: "Brands",
                column: "BrandStatusId",
                principalTable: "BrandStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EdgeBoxActivities_EdgeBoxStatuses_NewStatusId",
                table: "EdgeBoxActivities",
                column: "NewStatusId",
                principalTable: "EdgeBoxStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EdgeBoxActivities_EdgeBoxStatuses_OldStatusId",
                table: "EdgeBoxActivities",
                column: "OldStatusId",
                principalTable: "EdgeBoxStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EdgeBoxes_EdgeBoxLocation_EdgeBoxLocationId",
                table: "EdgeBoxes",
                column: "EdgeBoxLocationId",
                principalTable: "EdgeBoxLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EdgeBoxes_EdgeBoxStatuses_EdgeBoxStatusId",
                table: "EdgeBoxes",
                column: "EdgeBoxStatusId",
                principalTable: "EdgeBoxStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EdgeBoxInstalls_EdgeBoxInstallStatuses_EdgeBoxInstallStatusId",
                table: "EdgeBoxInstalls",
                column: "EdgeBoxInstallStatusId",
                principalTable: "EdgeBoxInstallStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeStatuses_EmployeeStatusId",
                table: "Employees",
                column: "EmployeeStatusId",
                principalTable: "EmployeeStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Evidences_EvidenceTypes_EvidenceTypeId",
                table: "Evidences",
                column: "EvidenceTypeId",
                principalTable: "EvidenceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId",
                principalTable: "NotificationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestActivities_RequestStatuses_NewStatusId",
                table: "RequestActivities",
                column: "NewStatusId",
                principalTable: "RequestStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestActivities_RequestStatuses_OldStatusId",
                table: "RequestActivities",
                column: "OldStatusId",
                principalTable: "RequestStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_RequestStatuses_RequestStatusId",
                table: "Requests",
                column: "RequestStatusId",
                principalTable: "RequestStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_RequestTypes_RequestTypeId",
                table: "Requests",
                column: "RequestTypeId",
                principalTable: "RequestTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_ShopStatuses_ShopStatusId",
                table: "Shops",
                column: "ShopStatusId",
                principalTable: "ShopStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
