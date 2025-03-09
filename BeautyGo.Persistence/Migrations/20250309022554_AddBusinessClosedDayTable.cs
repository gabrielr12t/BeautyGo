using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyGo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessClosedDayTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessWorkingHours_Business_BeautyBusinessId",
                schema: "AvailableHours",
                table: "BusinessWorkingHours");

            migrationBuilder.DropColumn(
                name: "Date",
                schema: "AvailableHours",
                table: "ProfessionalAvailabilities");

            migrationBuilder.RenameColumn(
                name: "OpenTime",
                schema: "AvailableHours",
                table: "BusinessWorkingHours",
                newName: "OpeningTime");

            migrationBuilder.RenameColumn(
                name: "CloseTime",
                schema: "AvailableHours",
                table: "BusinessWorkingHours",
                newName: "ClosingTime");

            migrationBuilder.RenameColumn(
                name: "BeautyBusinessId",
                schema: "AvailableHours",
                table: "BusinessWorkingHours",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_BusinessWorkingHours_BeautyBusinessId",
                schema: "AvailableHours",
                table: "BusinessWorkingHours",
                newName: "IX_BusinessWorkingHours_BusinessId");

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                schema: "AvailableHours",
                table: "ProfessionalAvailabilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BusinessClosedDays",
                schema: "AvailableHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClosedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessClosedDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessClosedDays_Business_BusinessId",
                        column: x => x.BusinessId,
                        principalSchema: "Businesses",
                        principalTable: "Business",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessClosedDays_BusinessId",
                schema: "AvailableHours",
                table: "BusinessClosedDays",
                column: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessWorkingHours_Business_BusinessId",
                schema: "AvailableHours",
                table: "BusinessWorkingHours",
                column: "BusinessId",
                principalSchema: "Businesses",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessWorkingHours_Business_BusinessId",
                schema: "AvailableHours",
                table: "BusinessWorkingHours");

            migrationBuilder.DropTable(
                name: "BusinessClosedDays",
                schema: "AvailableHours");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                schema: "AvailableHours",
                table: "ProfessionalAvailabilities");

            migrationBuilder.RenameColumn(
                name: "OpeningTime",
                schema: "AvailableHours",
                table: "BusinessWorkingHours",
                newName: "OpenTime");

            migrationBuilder.RenameColumn(
                name: "ClosingTime",
                schema: "AvailableHours",
                table: "BusinessWorkingHours",
                newName: "CloseTime");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                schema: "AvailableHours",
                table: "BusinessWorkingHours",
                newName: "BeautyBusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_BusinessWorkingHours_BusinessId",
                schema: "AvailableHours",
                table: "BusinessWorkingHours",
                newName: "IX_BusinessWorkingHours_BeautyBusinessId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                schema: "AvailableHours",
                table: "ProfessionalAvailabilities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessWorkingHours_Business_BeautyBusinessId",
                schema: "AvailableHours",
                table: "BusinessWorkingHours",
                column: "BeautyBusinessId",
                principalSchema: "Businesses",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
