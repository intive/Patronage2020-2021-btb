using Microsoft.EntityFrameworkCore.Migrations;

namespace BTB.Persistence.Migrations
{
    public partial class KlineIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Klines",
                table: "Klines");

            migrationBuilder.DropIndex(
                name: "IX_Klines_SymbolPairId",
                table: "Klines");

            migrationBuilder.AlterColumn<decimal>(
                name: "Volume",
                table: "Klines",
                type: "decimal(18, 9)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16, 8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OpenPrice",
                table: "Klines",
                type: "decimal(18, 9)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16, 5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "LowestPrice",
                table: "Klines",
                type: "decimal(18, 9)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16, 5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "HighestPrice",
                table: "Klines",
                type: "decimal(18, 9)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16, 5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ClosePrice",
                table: "Klines",
                type: "decimal(18, 9)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16, 5)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Klines",
                table: "Klines",
                column: "Id")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_Klines_DurationTimestamp",
                table: "Klines",
                column: "DurationTimestamp")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_Klines_SymbolPairId",
                table: "Klines",
                column: "SymbolPairId")
                .Annotation("SqlServer:Clustered", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Klines",
                table: "Klines");

            migrationBuilder.DropIndex(
                name: "IX_Klines_DurationTimestamp",
                table: "Klines");

            migrationBuilder.DropIndex(
                name: "IX_Klines_SymbolPairId",
                table: "Klines");

            migrationBuilder.AlterColumn<decimal>(
                name: "Volume",
                table: "Klines",
                type: "decimal(16, 8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 9)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OpenPrice",
                table: "Klines",
                type: "decimal(16, 5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 9)");

            migrationBuilder.AlterColumn<decimal>(
                name: "LowestPrice",
                table: "Klines",
                type: "decimal(16, 5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 9)");

            migrationBuilder.AlterColumn<decimal>(
                name: "HighestPrice",
                table: "Klines",
                type: "decimal(16, 5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 9)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ClosePrice",
                table: "Klines",
                type: "decimal(16, 5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 9)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Klines",
                table: "Klines",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Klines_SymbolPairId",
                table: "Klines",
                column: "SymbolPairId");
        }
    }
}
