using Microsoft.EntityFrameworkCore.Migrations;

namespace BTB.Persistence.Migrations
{
    public partial class SymbolKlineTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Symbols",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SymbolName = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symbols", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Klines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpenTimestamp = table.Column<string>(nullable: true),
                    CloseTimestamp = table.Column<string>(nullable: true),
                    OpenPrice = table.Column<string>(nullable: true),
                    ClosePrice = table.Column<string>(nullable: true),
                    LowestPrice = table.Column<string>(nullable: true),
                    HighestPrice = table.Column<string>(nullable: true),
                    BuySymbolId = table.Column<int>(nullable: false),
                    SellSymbolId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Klines_Symbols_BuySymbolId",
                        column: x => x.BuySymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Klines_Symbols_SellSymbolId",
                        column: x => x.SellSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Klines_BuySymbolId",
                table: "Klines",
                column: "BuySymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_Klines_SellSymbolId",
                table: "Klines",
                column: "SellSymbolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Klines");

            migrationBuilder.DropTable(
                name: "Symbols");
        }
    }
}
