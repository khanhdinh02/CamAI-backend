using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEvidenceImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uri",
                table: "Evidences");

            migrationBuilder.AddColumn<string>(
                name: "EdgeBoxPath",
                table: "Evidences",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Evidences",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "EdgeBoxInstallActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EdgeBoxInstallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldStatus = table.Column<int>(type: "int", nullable: false),
                    NewStatus = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "IX_Evidences_ImageId",
                table: "Evidences",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EdgeBoxInstallActivities_EdgeBoxInstallId",
                table: "EdgeBoxInstallActivities",
                column: "EdgeBoxInstallId");

            migrationBuilder.CreateIndex(
                name: "IX_EdgeBoxInstallActivities_ModifiedById",
                table: "EdgeBoxInstallActivities",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Evidences_Image_ImageId",
                table: "Evidences",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evidences_Image_ImageId",
                table: "Evidences");

            migrationBuilder.DropTable(
                name: "EdgeBoxInstallActivities");

            migrationBuilder.DropIndex(
                name: "IX_Evidences_ImageId",
                table: "Evidences");

            migrationBuilder.DropColumn(
                name: "EdgeBoxPath",
                table: "Evidences");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Evidences");

            migrationBuilder.AddColumn<string>(
                name: "Uri",
                table: "Evidences",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
