using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyGo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AjustandoUserTPT1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Customers_CustomerId",
                schema: "Appointments",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Professionals_ProfessionalId",
                schema: "Appointments",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Customers_CustomerId",
                schema: "Appointments",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionalAvailabilities_Professionals_ProfessionalId",
                schema: "AvailableHours",
                table: "ProfessionalAvailabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionalServices_Professionals_ProfessionalId",
                schema: "Professionals",
                table: "ProfessionalServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Business_BeautyBusinessId",
                schema: "Businesses",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_WaitingLists_Customers_CustomerId",
                schema: "Appointments",
                table: "WaitingLists");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "Users");

            migrationBuilder.DropTable(
                name: "Professionals",
                schema: "Users");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "User",
                newSchema: "Users");

            migrationBuilder.RenameColumn(
                name: "BeautyBusinessId",
                schema: "Businesses",
                table: "Services",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_Services_BeautyBusinessId",
                schema: "Businesses",
                table: "Services",
                newName: "IX_Services_BusinessId");

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                schema: "Users",
                table: "User",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserType",
                schema: "Users",
                table: "User",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_User_BusinessId",
                schema: "Users",
                table: "User",
                column: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_User_CustomerId",
                schema: "Appointments",
                table: "Appointment",
                column: "CustomerId",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_User_ProfessionalId",
                schema: "Appointments",
                table: "Appointment",
                column: "ProfessionalId",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_User_CustomerId",
                schema: "Appointments",
                table: "Feedbacks",
                column: "CustomerId",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionalAvailabilities_User_ProfessionalId",
                schema: "AvailableHours",
                table: "ProfessionalAvailabilities",
                column: "ProfessionalId",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionalServices_User_ProfessionalId",
                schema: "Professionals",
                table: "ProfessionalServices",
                column: "ProfessionalId",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Business_BusinessId",
                schema: "Businesses",
                table: "Services",
                column: "BusinessId",
                principalSchema: "Businesses",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Business_BusinessId",
                schema: "Users",
                table: "User",
                column: "BusinessId",
                principalSchema: "Businesses",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WaitingLists_User_CustomerId",
                schema: "Appointments",
                table: "WaitingLists",
                column: "CustomerId",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_User_CustomerId",
                schema: "Appointments",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_User_ProfessionalId",
                schema: "Appointments",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_User_CustomerId",
                schema: "Appointments",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionalAvailabilities_User_ProfessionalId",
                schema: "AvailableHours",
                table: "ProfessionalAvailabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionalServices_User_ProfessionalId",
                schema: "Professionals",
                table: "ProfessionalServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Business_BusinessId",
                schema: "Businesses",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Business_BusinessId",
                schema: "Users",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_WaitingLists_User_CustomerId",
                schema: "Appointments",
                table: "WaitingLists");

            migrationBuilder.DropIndex(
                name: "IX_User_BusinessId",
                schema: "Users",
                table: "User");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                schema: "Users",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserType",
                schema: "Users",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                schema: "Users",
                newName: "User");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                schema: "Businesses",
                table: "Services",
                newName: "BeautyBusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_Services_BusinessId",
                schema: "Businesses",
                table: "Services",
                newName: "IX_Services_BeautyBusinessId");

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Professionals",
                schema: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professionals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Professionals_Business_BusinessId",
                        column: x => x.BusinessId,
                        principalSchema: "Businesses",
                        principalTable: "Business",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Professionals_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_BusinessId",
                schema: "Users",
                table: "Professionals",
                column: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Customers_CustomerId",
                schema: "Appointments",
                table: "Appointment",
                column: "CustomerId",
                principalSchema: "Users",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Professionals_ProfessionalId",
                schema: "Appointments",
                table: "Appointment",
                column: "ProfessionalId",
                principalSchema: "Users",
                principalTable: "Professionals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Customers_CustomerId",
                schema: "Appointments",
                table: "Feedbacks",
                column: "CustomerId",
                principalSchema: "Users",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionalAvailabilities_Professionals_ProfessionalId",
                schema: "AvailableHours",
                table: "ProfessionalAvailabilities",
                column: "ProfessionalId",
                principalSchema: "Users",
                principalTable: "Professionals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionalServices_Professionals_ProfessionalId",
                schema: "Professionals",
                table: "ProfessionalServices",
                column: "ProfessionalId",
                principalSchema: "Users",
                principalTable: "Professionals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Business_BeautyBusinessId",
                schema: "Businesses",
                table: "Services",
                column: "BeautyBusinessId",
                principalSchema: "Businesses",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WaitingLists_Customers_CustomerId",
                schema: "Appointments",
                table: "WaitingLists",
                column: "CustomerId",
                principalSchema: "Users",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
