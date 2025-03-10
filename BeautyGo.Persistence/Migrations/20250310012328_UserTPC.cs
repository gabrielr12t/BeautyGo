using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyGo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UserTPC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditEntries_Users_UserId",
                table: "AuditEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Business_Users_OwnerId",
                schema: "Businesses",
                table: "Business");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_Id",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailTokens_Users_UserId",
                schema: "User",
                table: "EmailTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Professionals_Users_Id",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPasswords_Users_UserId",
                schema: "User",
                table: "UserPasswords");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRolesMapping_Users_UserId",
                schema: "User",
                table: "UserRolesMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersAddresses_Users_UserId",
                schema: "User",
                table: "UsersAddresses");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "User");

            migrationBuilder.AddColumn<Guid>(
                name: "BillingAddressId",
                schema: "Professionals",
                table: "Professionals",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CannotLoginUntilDate",
                schema: "Professionals",
                table: "Professionals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                schema: "Professionals",
                table: "Professionals",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Professionals",
                table: "Professionals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                schema: "Professionals",
                table: "Professionals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "Professionals",
                table: "Professionals",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                schema: "Professionals",
                table: "Professionals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EmailToRevalidate",
                schema: "Professionals",
                table: "Professionals",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "Professionals",
                table: "Professionals",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Professionals",
                table: "Professionals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActivityDate",
                schema: "Professionals",
                table: "Professionals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastIpAddress",
                schema: "Professionals",
                table: "Professionals",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDate",
                schema: "Professionals",
                table: "Professionals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "Professionals",
                table: "Professionals",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "MustChangePassword",
                schema: "Professionals",
                table: "Professionals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "Professionals",
                table: "Professionals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShippingAddressId",
                schema: "Professionals",
                table: "Professionals",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BillingAddressId",
                schema: "Users",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CannotLoginUntilDate",
                schema: "Users",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                schema: "Users",
                table: "Customers",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Users",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                schema: "Users",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "Users",
                table: "Customers",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                schema: "Users",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EmailToRevalidate",
                schema: "Users",
                table: "Customers",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "Users",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Users",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActivityDate",
                schema: "Users",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastIpAddress",
                schema: "Users",
                table: "Customers",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDate",
                schema: "Users",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "Users",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "MustChangePassword",
                schema: "Users",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "Users",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShippingAddressId",
                schema: "Users",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_Cpf",
                schema: "Professionals",
                table: "Professionals",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_Email",
                schema: "Professionals",
                table: "Professionals",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_LastLoginDate",
                schema: "Professionals",
                table: "Professionals",
                column: "LastLoginDate");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Professionals_Cpf",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropIndex(
                name: "IX_Professionals_Email",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropIndex(
                name: "IX_Professionals_LastLoginDate",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Cpf",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Email",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_LastLoginDate",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "BillingAddressId",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "CannotLoginUntilDate",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "Cpf",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "EmailToRevalidate",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "LastActivityDate",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "LastIpAddress",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "LastLoginDate",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "MustChangePassword",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "ShippingAddressId",
                schema: "Professionals",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "BillingAddressId",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CannotLoginUntilDate",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Cpf",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "EmailToRevalidate",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastActivityDate",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastIpAddress",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastLoginDate",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "MustChangePassword",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "Users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ShippingAddressId",
                schema: "Users",
                table: "Customers");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "User",
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
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Cpf",
                schema: "User",
                table: "Users",
                column: "Cpf",
                unique: true,
                filter: "[Cpf] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "User",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastLoginDate",
                schema: "User",
                table: "Users",
                column: "LastLoginDate");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditEntries_Users_UserId",
                table: "AuditEntries",
                column: "UserId",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Business_Users_OwnerId",
                schema: "Businesses",
                table: "Business",
                column: "OwnerId",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_Id",
                schema: "Users",
                table: "Customers",
                column: "Id",
                principalSchema: "User",
                principalTable: "Users",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Professionals_Users_Id",
                schema: "Professionals",
                table: "Professionals",
                column: "Id",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPasswords_Users_UserId",
                schema: "User",
                table: "UserPasswords",
                column: "UserId",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRolesMapping_Users_UserId",
                schema: "User",
                table: "UserRolesMapping",
                column: "UserId",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAddresses_Users_UserId",
                schema: "User",
                table: "UsersAddresses",
                column: "UserId",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
