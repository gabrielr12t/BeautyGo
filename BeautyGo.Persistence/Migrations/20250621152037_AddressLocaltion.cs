using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace BeautyGo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddressLocaltion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Point>(
                name: "Location",
                schema: "Common",
                table: "Addresses",
                type: "geography",
                nullable: true);

            migrationBuilder.Sql(
                "CREATE SPATIAL INDEX IX_Addresses_Location ON [Common].[Addresses](Location);"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                schema: "Common",
                table: "Addresses");
        }
    }
}
