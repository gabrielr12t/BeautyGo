using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyGo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdjustsmentsInBusiness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BeautyBusinessPicturies_BeautyBusiness_BeautyBusinessId",
                schema: "Business",
                table: "BeautyBusinessPicturies");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessWorkingHours_BeautyBusiness_BeautyBusinessId",
                schema: "AvailableHours",
                table: "BusinessWorkingHours");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailTokens_BeautyBusiness_BusinessId",
                schema: "Business",
                table: "EmailTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Professionals_BeautyBusiness_BusinessId",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_BeautyBusiness_BeautyBusinessId",
                schema: "Business",
                table: "Services");

            migrationBuilder.DropTable(
                name: "BeautyBusiness",
                schema: "Business");

            migrationBuilder.EnsureSchema(
                name: "Businesses");

            migrationBuilder.RenameTable(
                name: "Services",
                schema: "Business",
                newName: "Services",
                newSchema: "Businesses");

            migrationBuilder.RenameTable(
                name: "EmailTokens",
                schema: "Business",
                newName: "EmailTokens",
                newSchema: "Businesses");

            migrationBuilder.RenameTable(
                name: "BeautyBusinessPicturies",
                schema: "Business",
                newName: "BeautyBusinessPicturies",
                newSchema: "Businesses");

            migrationBuilder.CreateTable(
                name: "Business",
                schema: "Businesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    HomePageTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HomePageDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Cnpj = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DocumentValidated = table.Column<bool>(type: "bit", nullable: false),
                    Host = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Business_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "Common",
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Business_Users_CreatedId",
                        column: x => x.CreatedId,
                        principalSchema: "User",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Business_AddressId",
                schema: "Businesses",
                table: "Business",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Business_Cnpj",
                schema: "Businesses",
                table: "Business",
                column: "Cnpj",
                unique: true,
                filter: "[Cnpj] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Business_CreatedId",
                schema: "Businesses",
                table: "Business",
                column: "CreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Business_Name",
                schema: "Businesses",
                table: "Business",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Business_Phone",
                schema: "Businesses",
                table: "Business",
                column: "Phone",
                unique: true,
                filter: "[Phone] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Business_Url",
                schema: "Businesses",
                table: "Business",
                column: "Url",
                unique: true,
                filter: "[Url] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BeautyBusinessPicturies_Business_BeautyBusinessId",
                schema: "Businesses",
                table: "BeautyBusinessPicturies",
                column: "BeautyBusinessId",
                principalSchema: "Businesses",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessWorkingHours_Business_BeautyBusinessId",
                schema: "AvailableHours",
                table: "BusinessWorkingHours",
                column: "BeautyBusinessId",
                principalSchema: "Businesses",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTokens_Business_BusinessId",
                schema: "Businesses",
                table: "EmailTokens",
                column: "BusinessId",
                principalSchema: "Businesses",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Professionals_Business_BusinessId",
                schema: "Professionals",
                table: "Professionals",
                column: "BusinessId",
                principalSchema: "Businesses",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Business_BeautyBusinessId",
                schema: "Businesses",
                table: "Services",
                column: "BeautyBusinessId",
                principalSchema: "Businesses",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BeautyBusinessPicturies_Business_BeautyBusinessId",
                schema: "Businesses",
                table: "BeautyBusinessPicturies");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessWorkingHours_Business_BeautyBusinessId",
                schema: "AvailableHours",
                table: "BusinessWorkingHours");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailTokens_Business_BusinessId",
                schema: "Businesses",
                table: "EmailTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Professionals_Business_BusinessId",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Business_BeautyBusinessId",
                schema: "Businesses",
                table: "Services");

            migrationBuilder.DropTable(
                name: "Business",
                schema: "Businesses");

            migrationBuilder.EnsureSchema(
                name: "Business");

            migrationBuilder.RenameTable(
                name: "Services",
                schema: "Businesses",
                newName: "Services",
                newSchema: "Business");

            migrationBuilder.RenameTable(
                name: "EmailTokens",
                schema: "Businesses",
                newName: "EmailTokens",
                newSchema: "Business");

            migrationBuilder.RenameTable(
                name: "BeautyBusinessPicturies",
                schema: "Businesses",
                newName: "BeautyBusinessPicturies",
                newSchema: "Business");

            migrationBuilder.CreateTable(
                name: "BeautyBusiness",
                schema: "Business",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cnpj = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Code = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    HomePageDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HomePageTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Host = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeautyBusiness", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeautyBusiness_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "Common",
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BeautyBusiness_Users_CreatedId",
                        column: x => x.CreatedId,
                        principalSchema: "User",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BeautyBusiness_AddressId",
                schema: "Business",
                table: "BeautyBusiness",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_BeautyBusiness_Cnpj",
                schema: "Business",
                table: "BeautyBusiness",
                column: "Cnpj",
                unique: true,
                filter: "[Cnpj] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BeautyBusiness_CreatedId",
                schema: "Business",
                table: "BeautyBusiness",
                column: "CreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_BeautyBusiness_Name",
                schema: "Business",
                table: "BeautyBusiness",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BeautyBusiness_Phone",
                schema: "Business",
                table: "BeautyBusiness",
                column: "Phone",
                unique: true,
                filter: "[Phone] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BeautyBusiness_Url",
                schema: "Business",
                table: "BeautyBusiness",
                column: "Url",
                unique: true,
                filter: "[Url] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BeautyBusinessPicturies_BeautyBusiness_BeautyBusinessId",
                schema: "Business",
                table: "BeautyBusinessPicturies",
                column: "BeautyBusinessId",
                principalSchema: "Business",
                principalTable: "BeautyBusiness",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessWorkingHours_BeautyBusiness_BeautyBusinessId",
                schema: "AvailableHours",
                table: "BusinessWorkingHours",
                column: "BeautyBusinessId",
                principalSchema: "Business",
                principalTable: "BeautyBusiness",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Professionals_BeautyBusiness_BusinessId",
                schema: "Professionals",
                table: "Professionals",
                column: "BusinessId",
                principalSchema: "Business",
                principalTable: "BeautyBusiness",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_BeautyBusiness_BeautyBusinessId",
                schema: "Business",
                table: "Services",
                column: "BeautyBusinessId",
                principalSchema: "Business",
                principalTable: "BeautyBusiness",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
