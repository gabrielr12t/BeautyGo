using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyGo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdjustmentsToBackgroundService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sent",
                schema: "Events",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Processed",
                schema: "Business",
                table: "BeautyBusiness");

            migrationBuilder.AddColumn<int>(
                name: "Attempts",
                schema: "Events",
                table: "Event",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EventSource",
                schema: "Events",
                table: "Event",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Executed",
                schema: "Events",
                table: "Event",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "Events",
                table: "Event",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attempts",
                schema: "Events",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "EventSource",
                schema: "Events",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Executed",
                schema: "Events",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Events",
                table: "Event");

            migrationBuilder.AddColumn<bool>(
                name: "Sent",
                schema: "Events",
                table: "Event",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Processed",
                schema: "Business",
                table: "BeautyBusiness",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
