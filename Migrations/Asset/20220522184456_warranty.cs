using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetProject.Migrations.Asset
{
    public partial class warranty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssetWarranties_AssetId",
                table: "AssetWarranties");

            migrationBuilder.CreateIndex(
                name: "IX_AssetWarranties_AssetId",
                table: "AssetWarranties",
                column: "AssetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssetWarranties_AssetId",
                table: "AssetWarranties");

            migrationBuilder.CreateIndex(
                name: "IX_AssetWarranties_AssetId",
                table: "AssetWarranties",
                column: "AssetId",
                unique: true);
        }
    }
}
