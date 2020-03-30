using Microsoft.EntityFrameworkCore.Migrations;

namespace BTB.Persistence.Migrations
{
    public partial class AddedFavoriteSymbolPair : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoriteSymbolPairs",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(nullable: false),
                    SymbolPairId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteSymbolPairs", x => new { x.ApplicationUserId, x.SymbolPairId });
                    table.ForeignKey(
                        name: "FK_FavoriteSymbolPairs_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FavoriteSymbolPairs_SymbolPairs_SymbolPairId",
                        column: x => x.SymbolPairId,
                        principalTable: "SymbolPairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteSymbolPairs_SymbolPairId",
                table: "FavoriteSymbolPairs",
                column: "SymbolPairId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteSymbolPairs");
        }
    }
}
