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

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_ImageId",
                table: "Evidences",
                column: "ImageId");

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
