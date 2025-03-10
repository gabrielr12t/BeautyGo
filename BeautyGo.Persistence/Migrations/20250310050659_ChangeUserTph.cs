using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyGo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserTph : Migration
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
                name: "FK_Professionals_Business_BusinessId",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionalServices_Professionals_ProfessionalId",
                schema: "Professionals",
                table: "ProfessionalServices");

            migrationBuilder.DropForeignKey(
                name: "FK_WaitingLists_Customers_CustomerId",
                schema: "Appointments",
                table: "WaitingLists");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Professionals",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropIndex(
                name: "IX_Professionals_Cpf",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "BillingAddressId",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "ShippingAddressId",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.RenameTable(
                name: "Professionals",
                schema: "Professionals",
                newName: "User",
                newSchema: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_Professionals_LastLoginDate",
                schema: "Users",
                table: "User",
                newName: "IX_User_LastLoginDate");

            migrationBuilder.RenameIndex(
                name: "IX_Professionals_Email",
                schema: "Users",
                table: "User",
                newName: "IX_User_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Professionals_BusinessId",
                schema: "Users",
                table: "User",
                newName: "IX_User_BusinessId");

            migrationBuilder.AlterColumn<Guid>(
                name: "BusinessId",
                schema: "Users",
                table: "User",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "UserType",
                schema: "Users",
                table: "User",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                schema: "Users",
                table: "User",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Cpf",
                schema: "Users",
                table: "User",
                column: "Cpf",
                unique: true,
                filter: "[Cpf] IS NOT NULL");

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
                name: "FK_AuditEntries_User_UserId",
                table: "AuditEntries",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Business_User_OwnerId",
                schema: "Businesses",
                table: "Business",
                column: "OwnerId",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTokens_User_UserId",
                schema: "User",
                table: "EmailTokens",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_User_Business_BusinessId",
                schema: "Users",
                table: "User",
                column: "BusinessId",
                principalSchema: "Businesses",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPasswords_User_UserId",
                schema: "User",
                table: "UserPasswords",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRolesMapping_User_UserId",
                schema: "User",
                table: "UserRolesMapping",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAddresses_User_UserId",
                schema: "User",
                table: "UsersAddresses",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_AuditEntries_User_UserId",
                table: "AuditEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Business_User_OwnerId",
                schema: "Businesses",
                table: "Business");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailTokens_User_UserId",
                schema: "User",
                table: "EmailTokens");

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
                name: "FK_User_Business_BusinessId",
                schema: "Users",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPasswords_User_UserId",
                schema: "User",
                table: "UserPasswords");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRolesMapping_User_UserId",
                schema: "User",
                table: "UserRolesMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersAddresses_User_UserId",
                schema: "User",
                table: "UsersAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_WaitingLists_User_CustomerId",
                schema: "Appointments",
                table: "WaitingLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                schema: "Users",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Cpf",
                schema: "Users",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserType",
                schema: "Users",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                schema: "Users",
                newName: "Professionals",
                newSchema: "Professionals");

            migrationBuilder.RenameIndex(
                name: "IX_User_LastLoginDate",
                schema: "Professionals",
                table: "Professionals",
                newName: "IX_Professionals_LastLoginDate");

            migrationBuilder.RenameIndex(
                name: "IX_User_Email",
                schema: "Professionals",
                table: "Professionals",
                newName: "IX_Professionals_Email");

            migrationBuilder.RenameIndex(
                name: "IX_User_BusinessId",
                schema: "Professionals",
                table: "Professionals",
                newName: "IX_Professionals_BusinessId");

            migrationBuilder.AlterColumn<Guid>(
                name: "BusinessId",
                schema: "Professionals",
                table: "Professionals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BillingAddressId",
                schema: "Professionals",
                table: "Professionals",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShippingAddressId",
                schema: "Professionals",
                table: "Professionals",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Professionals",
                schema: "Professionals",
                table: "Professionals",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BillingAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CannotLoginUntilDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cpf = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    EmailToRevalidate = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastActivityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastIpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MustChangePassword = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_Cpf",
                schema: "Professionals",
                table: "Professionals",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Cpf",
                schema: "Users",
                table: "Customers",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                schema: "Users",
                table: "Customers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LastLoginDate",
                schema: "Users",
                table: "Customers",
                column: "LastLoginDate");

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
                principalSchema: "Professionals",
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
                principalSchema: "Professionals",
                principalTable: "Professionals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_ProfessionalServices_Professionals_ProfessionalId",
                schema: "Professionals",
                table: "ProfessionalServices",
                column: "ProfessionalId",
                principalSchema: "Professionals",
                principalTable: "Professionals",
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
