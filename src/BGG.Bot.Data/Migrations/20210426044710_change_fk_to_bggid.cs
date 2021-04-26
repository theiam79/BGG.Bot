using Microsoft.EntityFrameworkCore.Migrations;

namespace BGG.Bot.Data.Migrations
{
    public partial class change_fk_to_bggid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCollectionItem_CollectionItems_CollectionItemId",
                table: "UserCollectionItem");

            migrationBuilder.DropIndex(
                name: "IX_CollectionItems_BggId",
                table: "CollectionItems");

            migrationBuilder.RenameColumn(
                name: "CollectionItemId",
                table: "UserCollectionItem",
                newName: "BggId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCollectionItem_CollectionItemId",
                table: "UserCollectionItem",
                newName: "IX_UserCollectionItem_BggId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CollectionItems_BggId",
                table: "CollectionItems",
                column: "BggId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCollectionItem_CollectionItems_BggId",
                table: "UserCollectionItem",
                column: "BggId",
                principalTable: "CollectionItems",
                principalColumn: "BggId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCollectionItem_CollectionItems_BggId",
                table: "UserCollectionItem");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CollectionItems_BggId",
                table: "CollectionItems");

            migrationBuilder.RenameColumn(
                name: "BggId",
                table: "UserCollectionItem",
                newName: "CollectionItemId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCollectionItem_BggId",
                table: "UserCollectionItem",
                newName: "IX_UserCollectionItem_CollectionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionItems_BggId",
                table: "CollectionItems",
                column: "BggId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCollectionItem_CollectionItems_CollectionItemId",
                table: "UserCollectionItem",
                column: "CollectionItemId",
                principalTable: "CollectionItems",
                principalColumn: "CollectionItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
