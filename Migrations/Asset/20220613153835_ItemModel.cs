using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetProject.Migrations.Asset
{
    public partial class ItemModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
            name: "FK_Assets_Items_ItemId",
            table: "Assets");
            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Items_ItemId",
                table: "Assets",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Restrict);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
           name: "FK_Assets_Items_ItemId",
           table: "Assets",
            column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
           

            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Items_ItemId",
                table: "Assets"
               );
        }
    }
}
