using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyGo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterBusinessFilterProc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                GO 
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                ALTER PROCEDURE [Businesses].[GetFilteredBusinesses]
                    @Latitude FLOAT = NULL,
                    @Longitude FLOAT = NULL,
                    @RadiusKm FLOAT = NULL,
                    @Name NVARCHAR(100) = NULL,
                    @Description NVARCHAR(200) = NULL,
                    @PageNumber INT = 1,
                    @PageSize INT = 20
                AS
                BEGIN
                    SET NOCOUNT ON;
                 
                    WITH BusinessFilter AS (
                        SELECT 
                            b.Id AS BusinessId,
                            b.Name AS BusinessName,
                            b.Description, 
                            a.Id AS AddressId,
                            a.City,
                            a.State,
                            a.StateAbbreviation,
                            a.Neighborhood,
                            a.Number,
                            a.Street,
                            a.PostalCode, 
                            DistanceKm = 
                                CASE 
                                    WHEN @Latitude IS NOT NULL AND @Longitude IS NOT NULL THEN 
                                        (6371 * ACOS(
                                            COS(RADIANS(@Latitude)) * COS(RADIANS(a.Latitude)) *
                                            COS(RADIANS(a.Longitude) - RADIANS(@Longitude)) +
                                            SIN(RADIANS(@Latitude)) * SIN(RADIANS(a.Latitude))
                                        ))
                                    ELSE NULL
                                END
                        FROM [Businesses].[Business] b
                        INNER JOIN [Common].[Addresses] a ON b.AddressId = a.Id
                        WHERE 
                            b.IsActive = 1
                            AND b.EmailConfirmed = 1
                            AND b.Deleted IS NULL
                            AND b.DocumentValidated = 1
                            AND (@Name IS NULL OR b.Name LIKE '%' + CONVERT(NVARCHAR(100), @Name) + '%')
                            AND (@Description IS NULL OR b.Description LIKE '%' + CONVERT(NVARCHAR(200), @Description) + '%')
                            AND (
                                @RadiusKm IS NULL OR
                                (
                                    @Latitude IS NOT NULL AND 
                                    @Longitude IS NOT NULL AND
                                    (6371 * ACOS(
                                        COS(RADIANS(@Latitude)) * COS(RADIANS(a.Latitude)) *
                                        COS(RADIANS(a.Longitude) - RADIANS(@Longitude)) +
                                        SIN(RADIANS(@Latitude)) * SIN(RADIANS(a.Latitude))
                                    )) <= @RadiusKm
                                )
                            )
                    )
                
                    SELECT *
                    FROM BusinessFilter
                    ORDER BY 
                        CASE 
                            WHEN DistanceKm IS NULL THEN BusinessName
                            ELSE DistanceKm
                        END
                    OFFSET (@PageNumber - 1) * @PageSize ROWS
                    FETCH NEXT @PageSize ROWS ONLY;
                
                    SELECT COUNT(*) AS TotalCount
                    FROM (
                        SELECT 1 AS Dummy
                        FROM [Businesses].[Business] b
                        INNER JOIN [Common].[Addresses] a ON b.AddressId = a.Id
                        WHERE
                            b.IsActive = 1
                            AND b.EmailConfirmed = 1
                            AND b.Deleted IS NULL
                            AND b.DocumentValidated = 1
                            AND (@Name IS NULL OR b.Name LIKE '%' + CONVERT(NVARCHAR(100), @Name) + '%')
                            AND (@Description IS NULL OR b.Description LIKE '%' + CONVERT(NVARCHAR(200), @Description) + '%')
                            AND (
                                @RadiusKm IS NULL OR
                                (
                                    @Latitude IS NOT NULL AND 
                                    @Longitude IS NOT NULL AND
                                    (6371 * ACOS(
                                        COS(RADIANS(@Latitude)) * COS(RADIANS(a.Latitude)) *
                                        COS(RADIANS(a.Longitude) - RADIANS(@Longitude)) +
                                        SIN(RADIANS(@Latitude)) * SIN(RADIANS(a.Latitude))
                                    )) <= @RadiusKm
                                )
                            )
                    ) AS CountQuery;
                END
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
