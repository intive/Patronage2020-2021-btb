using Microsoft.EntityFrameworkCore.Migrations;

namespace BTB.Persistence.Migrations
{
    public partial class AddedEmailCountEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailCounts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DailyCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailCounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailCounts_Id",
                table: "EmailCounts",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailCounts");
        }
    }
}
