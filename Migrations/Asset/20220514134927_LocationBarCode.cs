using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetProject.Migrations.Asset
{
    public partial class LocationBarCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssetWarranties_AssetId",
                table: "AssetWarranties");

            migrationBuilder.AddColumn<string>(
                name: "BarCode",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "AssetMovements",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "MaintainanceStatuses",
                columns: new[] { "MaintainanceStatusId", "MaintainanceStatusTitle" },
                values: new object[,]
                {
                    { 1, "Scheduled" },
                    { 2, "In Progress" },
                    { 3, "On Hold" },
                    { 4, "Cancelled" },
                    { 5, "Completed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetWarranties_AssetId",
                table: "AssetWarranties",
                column: "AssetId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssetWarranties_AssetId",
                table: "AssetWarranties");

            migrationBuilder.DeleteData(
                table: "MaintainanceStatuses",
                keyColumn: "MaintainanceStatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MaintainanceStatuses",
                keyColumn: "MaintainanceStatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MaintainanceStatuses",
                keyColumn: "MaintainanceStatusId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MaintainanceStatuses",
                keyColumn: "MaintainanceStatusId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MaintainanceStatuses",
                keyColumn: "MaintainanceStatusId",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "BarCode",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "AssetMovements");

            migrationBuilder.CreateIndex(
                name: "IX_AssetWarranties_AssetId",
                table: "AssetWarranties",
                column: "AssetId");
        }
    }
}
