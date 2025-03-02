using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyGo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    INSERT INTO [User].[UserRoles] (ID, DESCRIPTION, CREATEDON) VALUES 
                    (NEWID(), 'GUEST', GETDATE()),
                    (NEWID(), 'CUSTOMER', GETDATE()),
                    (NEWID(), 'OWNER', GETDATE()),
                    (NEWID(), 'PROFESSIONAL', GETDATE()),
                    (NEWID(), 'SYSTEM_ADMIN', GETDATE());");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE [User].[UserRoles]");
        }
    }
}
