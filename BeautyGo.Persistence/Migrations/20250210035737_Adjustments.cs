using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyGo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Adjustments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                schema: "Business",
                table: "BeautyBusiness",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "Business",
                table: "BeautyBusiness",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                schema: "Business",
                table: "BeautyBusiness");

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                schema: "Business",
                table: "BeautyBusiness",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
