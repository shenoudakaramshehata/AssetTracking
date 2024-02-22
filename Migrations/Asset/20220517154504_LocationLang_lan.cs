using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetProject.Migrations.Asset
{
    public partial class LocationLang_lan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationLangtiude",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationLatitude",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);
            migrationBuilder.InsertData(
              table: "ActionLogs",
              columns: new[] { "ActionLogId", "ActionLogTitle" },
              values: new object[] { 20, "Add Asset Wrantty" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActionLogs",
                keyColumn: "ActionLogId",
                keyValue: 20);

            migrationBuilder.DropColumn(
                name: "LocationLangtiude",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LocationLatitude",
                table: "Locations");
        }
    }
}
