using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetProject.Migrations.Asset
{
    public partial class Seeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ItemTitle",
                table: "Items",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DocumentName",
                table: "assetDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "ActionLogs",
                keyColumn: "ActionLogId",
                keyValue: 20,
                column: "ActionLogTitle",
                value: "Add Asset Waranty");

            migrationBuilder.InsertData(
                table: "ActionLogs",
                columns: new[] { "ActionLogId", "ActionLogTitle" },
                values: new object[] { 21, "Deattach Asset Waranty" });

            migrationBuilder.UpdateData(
                table: "AssetStatuses",
                keyColumn: "AssetStatusId",
                keyValue: 9,
                column: "AssetStatusTitle",
                value: "InMaintainance");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActionLogs",
                keyColumn: "ActionLogId",
                keyValue: 21);

            migrationBuilder.AlterColumn<string>(
                name: "ItemTitle",
                table: "Items",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "DocumentName",
                table: "assetDocuments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "ActionLogs",
                keyColumn: "ActionLogId",
                keyValue: 20,
                column: "ActionLogTitle",
                value: "Add Asset Wrantty");

            migrationBuilder.UpdateData(
                table: "AssetStatuses",
                keyColumn: "AssetStatusId",
                keyValue: 9,
                column: "AssetStatusTitle",
                value: "InMaimtainance");
        }
    }
}
