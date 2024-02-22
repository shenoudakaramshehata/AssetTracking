using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetProject.Migrations.Asset
{
    public partial class AssetLeasingCostMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "LeasedCost",
                table: "AssetLeasings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "LeasedCost",
                table: "AssetLeasings");
        }
    }
}
