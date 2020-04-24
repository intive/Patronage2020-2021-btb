using Microsoft.EntityFrameworkCore.Migrations;

namespace BTB.Persistence.Migrations
{
    public partial class RemovedHeaderAndFooterAndAddedContentToEmailTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM EmailTemplates;", true);

            migrationBuilder.DropColumn(
                name: "Footer",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "Header",
                table: "EmailTemplates");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "EmailTemplates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM EmailTemplates;", true);

            migrationBuilder.DropColumn(
                name: "Content",
                table: "EmailTemplates");

            migrationBuilder.AddColumn<string>(
                name: "Footer",
                table: "EmailTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Header",
                table: "EmailTemplates",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
