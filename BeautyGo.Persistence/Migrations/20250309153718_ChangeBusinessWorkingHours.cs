using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyGo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBusinessWorkingHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessWorkingHours_Business_BusinessId",
                schema: "AvailableHours",
                table: "BusinessWorkingHours");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessWorkingHours_Business_BusinessId",
                schema: "AvailableHours",
                table: "BusinessWorkingHours",
                column: "BusinessId",
                principalSchema: "Businesses",
                principalTable: "Business",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessWorkingHours_Business_BusinessId",
                schema: "AvailableHours",
                table: "BusinessWorkingHours");

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
    }
}
