using Microsoft.EntityFrameworkCore.Migrations;

namespace BTB.Persistence.Migrations
{
    public partial class AddGamblePointAndUpdateFavoritePair : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteSymbolPairs_AspNetUsers_ApplicationUserId",
                table: "FavoriteSymbolPairs");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteSymbolPairs_SymbolPairs_SymbolPairId",
                table: "FavoriteSymbolPairs");

            migrationBuilder.CreateTable(
                name: "GamblePoints",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    FreePoints = table.Column<decimal>(type: "decimal(18, 9)", nullable: false),
                    SealedPoints = table.Column<decimal>(type: "decimal(18, 9)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamblePoints", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_GamblePoints_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteSymbolPairs_AspNetUsers_ApplicationUserId",
                table: "FavoriteSymbolPairs",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteSymbolPairs_SymbolPairs_SymbolPairId",
                table: "FavoriteSymbolPairs",
                column: "SymbolPairId",
                principalTable: "SymbolPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteSymbolPairs_AspNetUsers_ApplicationUserId",
                table: "FavoriteSymbolPairs");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteSymbolPairs_SymbolPairs_SymbolPairId",
                table: "FavoriteSymbolPairs");

            migrationBuilder.DropTable(
                name: "GamblePoints");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteSymbolPairs_AspNetUsers_ApplicationUserId",
                table: "FavoriteSymbolPairs",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteSymbolPairs_SymbolPairs_SymbolPairId",
                table: "FavoriteSymbolPairs",
                column: "SymbolPairId",
                principalTable: "SymbolPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
