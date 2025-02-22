using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyGo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdjustmentsValidationToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailTokens_BeautyBusiness_BeautyBusinessId",
                schema: "Business",
                table: "EmailTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailTokens_BeautyBusiness_BusinessId",
                schema: "Business",
                table: "EmailTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailTokens_Users_UserId",
                schema: "User",
                table: "EmailTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailTokens_Users_UserId1",
                schema: "User",
                table: "EmailTokens");

            migrationBuilder.DropIndex(
                name: "IX_EmailTokens_UserId1",
                schema: "User",
                table: "EmailTokens");

            migrationBuilder.DropIndex(
                name: "IX_EmailTokens_BeautyBusinessId",
                schema: "Business",
                table: "EmailTokens");

            migrationBuilder.DropColumn(
                name: "UserId1",
                schema: "User",
                table: "EmailTokens");

            migrationBuilder.DropColumn(
                name: "BeautyBusinessId",
                schema: "Business",
                table: "EmailTokens");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTokens_BeautyBusiness_BusinessId",
                schema: "Business",
                table: "EmailTokens",
                column: "BusinessId",
                principalSchema: "Business",
                principalTable: "BeautyBusiness",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTokens_Users_UserId",
                schema: "User",
                table: "EmailTokens",
                column: "UserId",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailTokens_BeautyBusiness_BusinessId",
                schema: "Business",
                table: "EmailTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailTokens_Users_UserId",
                schema: "User",
                table: "EmailTokens");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                schema: "User",
                table: "EmailTokens",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BeautyBusinessId",
                schema: "Business",
                table: "EmailTokens",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailTokens_UserId1",
                schema: "User",
                table: "EmailTokens",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTokens_BeautyBusinessId",
                schema: "Business",
                table: "EmailTokens",
                column: "BeautyBusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTokens_BeautyBusiness_BeautyBusinessId",
                schema: "Business",
                table: "EmailTokens",
                column: "BeautyBusinessId",
                principalSchema: "Business",
                principalTable: "BeautyBusiness",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTokens_BeautyBusiness_BusinessId",
                schema: "Business",
                table: "EmailTokens",
                column: "BusinessId",
                principalSchema: "Business",
                principalTable: "BeautyBusiness",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTokens_Users_UserId",
                schema: "User",
                table: "EmailTokens",
                column: "UserId",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTokens_Users_UserId1",
                schema: "User",
                table: "EmailTokens",
                column: "UserId1",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
