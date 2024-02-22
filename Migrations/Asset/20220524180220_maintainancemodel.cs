using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetProject.Migrations.Asset
{
    public partial class maintainancemodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetMaintainances_MaintainanceStatuses_MaintainanceStatusId",
                table: "AssetMaintainances");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetMaintainances_Technicians_TechnicianId",
                table: "AssetMaintainances");

            migrationBuilder.AlterColumn<int>(
                name: "TechnicianId",
                table: "AssetMaintainances",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MaintainanceStatusId",
                table: "AssetMaintainances",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AssetMaintainanceDueDate",
                table: "AssetMaintainances",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AssetMaintainanceDateCompleted",
                table: "AssetMaintainances",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetMaintainances_MaintainanceStatuses_MaintainanceStatusId",
                table: "AssetMaintainances",
                column: "MaintainanceStatusId",
                principalTable: "MaintainanceStatuses",
                principalColumn: "MaintainanceStatusId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetMaintainances_Technicians_TechnicianId",
                table: "AssetMaintainances",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "TechnicianId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetMaintainances_MaintainanceStatuses_MaintainanceStatusId",
                table: "AssetMaintainances");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetMaintainances_Technicians_TechnicianId",
                table: "AssetMaintainances");

            migrationBuilder.AlterColumn<int>(
                name: "TechnicianId",
                table: "AssetMaintainances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaintainanceStatusId",
                table: "AssetMaintainances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AssetMaintainanceDueDate",
                table: "AssetMaintainances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AssetMaintainanceDateCompleted",
                table: "AssetMaintainances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetMaintainances_MaintainanceStatuses_MaintainanceStatusId",
                table: "AssetMaintainances",
                column: "MaintainanceStatusId",
                principalTable: "MaintainanceStatuses",
                principalColumn: "MaintainanceStatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetMaintainances_Technicians_TechnicianId",
                table: "AssetMaintainances",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "TechnicianId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
