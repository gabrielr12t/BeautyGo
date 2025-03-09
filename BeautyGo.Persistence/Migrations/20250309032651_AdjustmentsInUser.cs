using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyGo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdjustmentsInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Business_Users_CreatedId",
                schema: "Businesses",
                table: "Business");

            migrationBuilder.RenameColumn(
                name: "CreatedId",
                schema: "Businesses",
                table: "Business",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Business_CreatedId",
                schema: "Businesses",
                table: "Business",
                newName: "IX_Business_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Business_Users_OwnerId",
                schema: "Businesses",
                table: "Business",
                column: "OwnerId",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Business_Users_OwnerId",
                schema: "Businesses",
                table: "Business");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                schema: "Businesses",
                table: "Business",
                newName: "CreatedId");

            migrationBuilder.RenameIndex(
                name: "IX_Business_OwnerId",
                schema: "Businesses",
                table: "Business",
                newName: "IX_Business_CreatedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Business_Users_CreatedId",
                schema: "Businesses",
                table: "Business",
                column: "CreatedId",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
