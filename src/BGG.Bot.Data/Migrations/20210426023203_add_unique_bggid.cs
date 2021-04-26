using Microsoft.EntityFrameworkCore.Migrations;

namespace BGG.Bot.Data.Migrations
{
    public partial class add_unique_bggid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CollectionItems_BggId",
                table: "CollectionItems",
                column: "BggId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CollectionItems_BggId",
                table: "CollectionItems");
        }
    }
}
