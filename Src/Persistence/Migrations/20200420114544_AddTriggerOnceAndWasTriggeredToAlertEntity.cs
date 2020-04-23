using Microsoft.EntityFrameworkCore.Migrations;

namespace BTB.Persistence.Migrations
{
    public partial class AddTriggerOnceAndWasTriggeredToAlertEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Alerts;", true);

            migrationBuilder.AddColumn<bool>(
                name: "TriggerOnce",
                table: "Alerts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WasTriggered",
                table: "Alerts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Alerts;", true);

            migrationBuilder.DropColumn(
                name: "TriggerOnce",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "WasTriggered",
                table: "Alerts");
        }
    }
}
