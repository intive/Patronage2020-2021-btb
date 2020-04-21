using Microsoft.EntityFrameworkCore.Migrations;

namespace BTB.Persistence.Migrations
{
    public partial class AddAdditionalValueColumnToAlertEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Alerts;", true);

            migrationBuilder.AddColumn<decimal>(
                name: "AdditionalValue",
                table: "Alerts",
                type: "decimal(18, 9)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalValue",
                table: "Alerts");
        }
    }
}
