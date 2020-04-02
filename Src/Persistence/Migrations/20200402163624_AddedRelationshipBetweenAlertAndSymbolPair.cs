using Microsoft.EntityFrameworkCore.Migrations;

namespace BTB.Persistence.Migrations
{
    public partial class AddedRelationshipBetweenAlertAndSymbolPair : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "Alerts");

            migrationBuilder.AddColumn<int>(
                name: "SymbolPairId",
                table: "Alerts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_SymbolPairId",
                table: "Alerts",
                column: "SymbolPairId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_SymbolPairs_SymbolPairId",
                table: "Alerts",
                column: "SymbolPairId",
                principalTable: "SymbolPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_SymbolPairs_SymbolPairId",
                table: "Alerts");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_SymbolPairId",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "SymbolPairId",
                table: "Alerts");

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "Alerts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }
    }
}
