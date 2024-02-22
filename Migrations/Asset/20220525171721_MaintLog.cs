using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetProject.Migrations.Asset
{
    public partial class MaintLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ActionLogs",
                columns: new[] { "ActionLogId", "ActionLogTitle" },
                values: new object[] { 22, "Edit Asset Maintainance" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActionLogs",
                keyColumn: "ActionLogId",
                keyValue: 22);
        }
    }
}
