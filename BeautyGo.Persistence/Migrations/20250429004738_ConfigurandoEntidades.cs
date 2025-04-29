using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyGo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurandoEntidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Event_EventId",
                schema: "EventErrors",
                table: "Event");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Event",
                schema: "EventErrors",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Error",
                table: "OutboxMessage");

            migrationBuilder.RenameTable(
                name: "UsersAddresses",
                schema: "User",
                newName: "UsersAddresses",
                newSchema: "Users");

            migrationBuilder.RenameTable(
                name: "UserRolesMapping",
                schema: "User",
                newName: "UserRolesMapping",
                newSchema: "Users");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "User",
                newName: "UserRoles",
                newSchema: "Users");

            migrationBuilder.RenameTable(
                name: "UserPasswords",
                schema: "User",
                newName: "UserPasswords",
                newSchema: "Users");

            migrationBuilder.RenameTable(
                name: "ProfessionalRequests",
                schema: "Business",
                newName: "ProfessionalRequests",
                newSchema: "Businesses");

            migrationBuilder.RenameTable(
                name: "EmailTokens",
                schema: "User",
                newName: "EmailTokens",
                newSchema: "Users");

            migrationBuilder.RenameTable(
                name: "Event",
                schema: "EventErrors",
                newName: "EventError",
                newSchema: "Events");

            migrationBuilder.RenameIndex(
                name: "IX_Event_EventId",
                schema: "Events",
                table: "EventError",
                newName: "IX_EventError_EventId");

            migrationBuilder.AddColumn<int>(
                name: "Attempts",
                table: "OutboxMessage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventError",
                schema: "Events",
                table: "EventError",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "OutboxMessageError",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutboxMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessageError", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutboxMessageError_OutboxMessage_OutboxMessageId",
                        column: x => x.OutboxMessageId,
                        principalTable: "OutboxMessage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessageError_OutboxMessageId",
                table: "OutboxMessageError",
                column: "OutboxMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventError_Event_EventId",
                schema: "Events",
                table: "EventError",
                column: "EventId",
                principalSchema: "Events",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventError_Event_EventId",
                schema: "Events",
                table: "EventError");

            migrationBuilder.DropTable(
                name: "OutboxMessageError");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventError",
                schema: "Events",
                table: "EventError");

            migrationBuilder.DropColumn(
                name: "Attempts",
                table: "OutboxMessage");

            migrationBuilder.EnsureSchema(
                name: "User");

            migrationBuilder.EnsureSchema(
                name: "EventErrors");

            migrationBuilder.EnsureSchema(
                name: "Business");

            migrationBuilder.RenameTable(
                name: "UsersAddresses",
                schema: "Users",
                newName: "UsersAddresses",
                newSchema: "User");

            migrationBuilder.RenameTable(
                name: "UserRolesMapping",
                schema: "Users",
                newName: "UserRolesMapping",
                newSchema: "User");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "Users",
                newName: "UserRoles",
                newSchema: "User");

            migrationBuilder.RenameTable(
                name: "UserPasswords",
                schema: "Users",
                newName: "UserPasswords",
                newSchema: "User");

            migrationBuilder.RenameTable(
                name: "ProfessionalRequests",
                schema: "Businesses",
                newName: "ProfessionalRequests",
                newSchema: "Business");

            migrationBuilder.RenameTable(
                name: "EmailTokens",
                schema: "Users",
                newName: "EmailTokens",
                newSchema: "User");

            migrationBuilder.RenameTable(
                name: "EventError",
                schema: "Events",
                newName: "Event",
                newSchema: "EventErrors");

            migrationBuilder.RenameIndex(
                name: "IX_EventError_EventId",
                schema: "EventErrors",
                table: "Event",
                newName: "IX_Event_EventId");

            migrationBuilder.AddColumn<string>(
                name: "Error",
                table: "OutboxMessage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Event",
                schema: "EventErrors",
                table: "Event",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Event_EventId",
                schema: "EventErrors",
                table: "Event",
                column: "EventId",
                principalSchema: "Events",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
