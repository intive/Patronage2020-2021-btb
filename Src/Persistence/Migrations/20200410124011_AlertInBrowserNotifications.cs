using Microsoft.EntityFrameworkCore.Migrations;

namespace BTB.Persistence.Migrations
{
    public partial class AlertInBrowserNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SendInBrowser",
                table: "Alerts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendInBrowser",
                table: "Alerts");
        }
    }
}
