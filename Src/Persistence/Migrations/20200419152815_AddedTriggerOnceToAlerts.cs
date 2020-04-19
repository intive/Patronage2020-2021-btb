﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace BTB.Persistence.Migrations
{
    public partial class AddedTriggerOnceToAlerts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
            migrationBuilder.DropColumn(
                name: "TriggerOnce",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "WasTriggered",
                table: "Alerts");
        }
    }
}
