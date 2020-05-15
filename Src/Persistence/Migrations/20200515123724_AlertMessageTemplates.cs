using Microsoft.EntityFrameworkCore.Migrations;

namespace BTB.Persistence.Migrations
{
    public partial class AlertMessageTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Alerts;");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Alerts");

            migrationBuilder.AddColumn<int>(
                name: "MessageTemplateId",
                table: "Alerts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AlertMessageTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertMessageTemplates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_MessageTemplateId",
                table: "Alerts",
                column: "MessageTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_AlertMessageTemplates_MessageTemplateId",
                table: "Alerts",
                column: "MessageTemplateId",
                principalTable: "AlertMessageTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Alerts;");

            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_AlertMessageTemplates_MessageTemplateId",
                table: "Alerts");

            migrationBuilder.DropTable(
                name: "AlertMessageTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_MessageTemplateId",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "MessageTemplateId",
                table: "Alerts");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Alerts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
