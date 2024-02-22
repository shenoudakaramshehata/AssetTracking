using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetProject.Migrations.Asset
{
    public partial class AssetWarranty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetWarranties",
                columns: table => new
                {
                    WarrantyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Length = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetWarranties", x => x.WarrantyId);
                    table.ForeignKey(
                        name: "FK_AssetWarranties_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "ActionLogs",
                keyColumn: "ActionLogId",
                keyValue: 14,
                column: "ActionLogTitle",
                value: "Asset Lost");

            migrationBuilder.InsertData(
                table: "ActionLogs",
                columns: new[] { "ActionLogId", "ActionLogTitle" },
                values: new object[] { 19, "Asset Edited" });

            migrationBuilder.UpdateData(
                table: "ActionTypes",
                keyColumn: "ActionTypeId",
                keyValue: 1,
                column: "ActionTypeTitle",
                value: "Employee");

            migrationBuilder.UpdateData(
                table: "ActionTypes",
                keyColumn: "ActionTypeId",
                keyValue: 2,
                column: "ActionTypeTitle",
                value: "Department");

            migrationBuilder.CreateIndex(
                name: "IX_AssetWarranties_AssetId",
                table: "AssetWarranties",
                column: "AssetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetWarranties");

            migrationBuilder.DeleteData(
                table: "ActionLogs",
                keyColumn: "ActionLogId",
                keyValue: 19);

            migrationBuilder.UpdateData(
                table: "ActionLogs",
                keyColumn: "ActionLogId",
                keyValue: 14,
                column: "ActionLogTitle",
                value: "Repair Asset");

            migrationBuilder.UpdateData(
                table: "ActionTypes",
                keyColumn: "ActionTypeId",
                keyValue: 1,
                column: "ActionTypeTitle",
                value: "To Employee");

            migrationBuilder.UpdateData(
                table: "ActionTypes",
                keyColumn: "ActionTypeId",
                keyValue: 2,
                column: "ActionTypeTitle",
                value: "To Department");
        }
    }
}
