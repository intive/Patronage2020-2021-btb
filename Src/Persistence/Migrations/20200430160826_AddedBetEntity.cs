using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BTB.Persistence.Migrations
{
    public partial class AddedBetEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    SymbolPairId = table.Column<int>(nullable: false),
                    Points = table.Column<decimal>(type: "decimal(18, 9)", nullable: false),
                    LowerPriceThreshold = table.Column<decimal>(type: "decimal(18, 9)", nullable: false),
                    UpperPriceThreshold = table.Column<decimal>(type: "decimal(18, 9)", nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    RateType = table.Column<int>(nullable: false),
                    TimeInterval = table.Column<int>(nullable: false),
                    KlineOpenTimestamp = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bets_SymbolPairs_SymbolPairId",
                        column: x => x.SymbolPairId,
                        principalTable: "SymbolPairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bets_SymbolPairId",
                table: "Bets",
                column: "SymbolPairId");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_UserId",
                table: "Bets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bets");
        }
    }
}
