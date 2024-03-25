using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RenameImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Brands_Image_BannerId", table: "Brands");

            migrationBuilder.DropForeignKey(name: "FK_Brands_Image_LogoId", table: "Brands");

            migrationBuilder.DropForeignKey(name: "FK_Evidences_Image_ImageId", table: "Evidences");

            migrationBuilder.DropPrimaryKey(name: "PK_Image", table: "Image");

            migrationBuilder.RenameTable(name: "Image", schema: "dbo", newName: "Images", newSchema: "dbo");

            migrationBuilder.AddPrimaryKey(name: "PK_Images", table: "Images", column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Images_BannerId",
                table: "Brands",
                column: "BannerId",
                principalTable: "Images",
                principalColumn: "Id"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Images_LogoId",
                table: "Brands",
                column: "LogoId",
                principalTable: "Images",
                principalColumn: "Id"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Evidences_Images_ImageId",
                table: "Evidences",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Brands_Images_BannerId", table: "Brands");

            migrationBuilder.DropForeignKey(name: "FK_Brands_Images_LogoId", table: "Brands");

            migrationBuilder.DropForeignKey(name: "FK_Evidences_Images_ImageId", table: "Evidences");

            migrationBuilder.DropPrimaryKey(name: "PK_Images", table: "Images");

            migrationBuilder.RenameTable(name: "Images", schema: "dbo", newName: "Image", newSchema: "dbo");

            migrationBuilder.AddPrimaryKey(name: "PK_Image", table: "Image", column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Image_BannerId",
                table: "Brands",
                column: "BannerId",
                principalTable: "Image",
                principalColumn: "Id"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Image_LogoId",
                table: "Brands",
                column: "LogoId",
                principalTable: "Image",
                principalColumn: "Id"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Evidences_Image_ImageId",
                table: "Evidences",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id"
            );
        }
    }
}
