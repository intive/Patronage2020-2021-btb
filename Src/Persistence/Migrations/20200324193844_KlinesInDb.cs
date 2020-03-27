using Microsoft.EntityFrameworkCore.Migrations;

namespace BTB.Persistence.Migrations
{
    public partial class KlinesInDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Klines_Symbols_BuySymbolId",
                table: "Klines");

            migrationBuilder.DropForeignKey(
                name: "FK_Klines_Symbols_SellSymbolId",
                table: "Klines");

            migrationBuilder.DropIndex(
                name: "IX_Klines_BuySymbolId",
                table: "Klines");

            migrationBuilder.DropIndex(
                name: "IX_Klines_SellSymbolId",
                table: "Klines");

            migrationBuilder.DropColumn(
                name: "BuySymbolId",
                table: "Klines");

            migrationBuilder.DropColumn(
                name: "CloseTimestamp",
                table: "Klines");

            migrationBuilder.DropColumn(
                name: "SellSymbolId",
                table: "Klines");

            migrationBuilder.AlterColumn<long>(
                name: "OpenTimestamp",
                table: "Klines",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "OpenPrice",
                table: "Klines",
                type: "decimal(16, 5)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LowestPrice",
                table: "Klines",
                type: "decimal(16, 5)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "HighestPrice",
                table: "Klines",
                type: "decimal(16, 5)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ClosePrice",
                table: "Klines",
                type: "decimal(16, 5)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationTimestamp",
                table: "Klines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SymbolPairId",
                table: "Klines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Volume",
                table: "Klines",
                type: "decimal(16, 8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "SymbolPairs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuySymbolId = table.Column<int>(nullable: false),
                    SellSymbolId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymbolPairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymbolPairs_Symbols_BuySymbolId",
                        column: x => x.BuySymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SymbolPairs_Symbols_SellSymbolId",
                        column: x => x.SellSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Klines_SymbolPairId",
                table: "Klines",
                column: "SymbolPairId");

            migrationBuilder.CreateIndex(
                name: "IX_SymbolPairs_BuySymbolId",
                table: "SymbolPairs",
                column: "BuySymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_SymbolPairs_SellSymbolId",
                table: "SymbolPairs",
                column: "SellSymbolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Klines_SymbolPairs_SymbolPairId",
                table: "Klines",
                column: "SymbolPairId",
                principalTable: "SymbolPairs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Klines_SymbolPairs_SymbolPairId",
                table: "Klines");

            migrationBuilder.DropTable(
                name: "SymbolPairs");

            migrationBuilder.DropIndex(
                name: "IX_Klines_SymbolPairId",
                table: "Klines");

            migrationBuilder.DropColumn(
                name: "DurationTimestamp",
                table: "Klines");

            migrationBuilder.DropColumn(
                name: "SymbolPairId",
                table: "Klines");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Klines");

            migrationBuilder.AlterColumn<string>(
                name: "OpenTimestamp",
                table: "Klines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "OpenPrice",
                table: "Klines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(16, 5)");

            migrationBuilder.AlterColumn<string>(
                name: "LowestPrice",
                table: "Klines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(16, 5)");

            migrationBuilder.AlterColumn<string>(
                name: "HighestPrice",
                table: "Klines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(16, 5)");

            migrationBuilder.AlterColumn<string>(
                name: "ClosePrice",
                table: "Klines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(16, 5)");

            migrationBuilder.AddColumn<int>(
                name: "BuySymbolId",
                table: "Klines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CloseTimestamp",
                table: "Klines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SellSymbolId",
                table: "Klines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Klines_BuySymbolId",
                table: "Klines",
                column: "BuySymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_Klines_SellSymbolId",
                table: "Klines",
                column: "SellSymbolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Klines_Symbols_BuySymbolId",
                table: "Klines",
                column: "BuySymbolId",
                principalTable: "Symbols",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Klines_Symbols_SellSymbolId",
                table: "Klines",
                column: "SellSymbolId",
                principalTable: "Symbols",
                principalColumn: "Id");
        }
    }
}
