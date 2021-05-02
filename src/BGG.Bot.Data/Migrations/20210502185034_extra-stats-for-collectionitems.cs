using Microsoft.EntityFrameworkCore.Migrations;

namespace BGG.Bot.Data.Migrations
{
    public partial class extrastatsforcollectionitems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Rating",
                table: "UserCollectionItem",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "WishlistPriority",
                table: "UserCollectionItem",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "UserCollectionItem");

            migrationBuilder.DropColumn(
                name: "WishlistPriority",
                table: "UserCollectionItem");
        }
    }
}
