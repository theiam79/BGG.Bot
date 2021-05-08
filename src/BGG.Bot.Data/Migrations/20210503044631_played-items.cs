using Microsoft.EntityFrameworkCore.Migrations;

namespace BGG.Bot.Data.Migrations
{
    public partial class playeditems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCollectionItem_CollectionItems_BggId",
                table: "UserCollectionItem");

            migrationBuilder.DropTable(
                name: "CollectionItems");

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BggId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                    table.UniqueConstraint("AK_Items_BggId", x => x.BggId);
                });

            migrationBuilder.CreateTable(
                name: "UserPlayedItems",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    BggId = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlayedItems", x => new { x.UserId, x.BggId });
                    table.ForeignKey(
                        name: "FK_UserPlayedItems_Items_BggId",
                        column: x => x.BggId,
                        principalTable: "Items",
                        principalColumn: "BggId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPlayedItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPlayedItems_BggId",
                table: "UserPlayedItems",
                column: "BggId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCollectionItem_Items_BggId",
                table: "UserCollectionItem",
                column: "BggId",
                principalTable: "Items",
                principalColumn: "BggId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCollectionItem_Items_BggId",
                table: "UserCollectionItem");

            migrationBuilder.DropTable(
                name: "UserPlayedItems");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.CreateTable(
                name: "CollectionItems",
                columns: table => new
                {
                    CollectionItemId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BggId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionItems", x => x.CollectionItemId);
                    table.UniqueConstraint("AK_CollectionItems_BggId", x => x.BggId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UserCollectionItem_CollectionItems_BggId",
                table: "UserCollectionItem",
                column: "BggId",
                principalTable: "CollectionItems",
                principalColumn: "BggId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
