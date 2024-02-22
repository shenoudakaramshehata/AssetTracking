using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetProject.Migrations.Asset
{
    public partial class Techinancianmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Technicians",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Technicians_TenantId",
                table: "Technicians",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Technicians_Tenants_TenantId",
                table: "Technicians",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Technicians_Tenants_TenantId",
                table: "Technicians");

            migrationBuilder.DropIndex(
                name: "IX_Technicians_TenantId",
                table: "Technicians");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Technicians");
        }
    }
}
