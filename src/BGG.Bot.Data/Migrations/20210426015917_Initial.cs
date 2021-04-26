using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BGG.Bot.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DiscordId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    BggUsername = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserCollectionItem",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CollectionItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    Owned = table.Column<bool>(type: "INTEGER", nullable: false),
                    PreviouslyOwned = table.Column<bool>(type: "INTEGER", nullable: false),
                    ForTrade = table.Column<bool>(type: "INTEGER", nullable: false),
                    Want = table.Column<bool>(type: "INTEGER", nullable: false),
                    WantToPlay = table.Column<bool>(type: "INTEGER", nullable: false),
                    WantToBuy = table.Column<bool>(type: "INTEGER", nullable: false),
                    WishList = table.Column<bool>(type: "INTEGER", nullable: false),
                    PreOrdered = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCollectionItem", x => new { x.UserId, x.CollectionItemId });
                    table.ForeignKey(
                        name: "FK_UserCollectionItem_CollectionItems_CollectionItemId",
                        column: x => x.CollectionItemId,
                        principalTable: "CollectionItems",
                        principalColumn: "CollectionItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCollectionItem_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCollectionItem_CollectionItemId",
                table: "UserCollectionItem",
                column: "CollectionItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCollectionItem");

            migrationBuilder.DropTable(
                name: "CollectionItems");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
