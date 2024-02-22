using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetProject.Migrations.Asset
{
    public partial class AssetTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assets_AssetSerialNo",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_AssetTagId",
                table: "Assets");

          

            migrationBuilder.AlterColumn<string>(
                name: "AssetTagId",
                table: "Assets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AssetSerialNo",
                table: "Assets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetSerialNo",
                table: "Assets",
                column: "AssetSerialNo",
                unique: true,
                filter: "[AssetSerialNo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetTagId",
                table: "Assets",
                column: "AssetTagId",
                unique: true,
                filter: "[AssetTagId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assets_AssetSerialNo",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_AssetTagId",
                table: "Assets");

          

            migrationBuilder.AlterColumn<string>(
                name: "AssetTagId",
                table: "Assets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AssetSerialNo",
                table: "Assets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetSerialNo",
                table: "Assets",
                column: "AssetSerialNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetTagId",
                table: "Assets",
                column: "AssetTagId",
                unique: true);
        }
    }
}
