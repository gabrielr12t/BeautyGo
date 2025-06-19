using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyGo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateBusinessCalculateDistance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [Businesses].[GetFilteredBusinesses]
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
                
                    -- CTE com cálculo de distância e filtros
                    WITH FilteredBusinesses AS (
                        SELECT 
                            b.Id,
                            b.Name,
                            b.Description,
                            b.Email, 
                            b.IsActive,
                            b.CreatedOn,
                            a.Street,
                            a.Number,
                            a.City,
                            a.State, 
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
                            AND (@Name IS NULL OR b.Name LIKE '%' + @Name + '%')
                            AND (@Description IS NULL OR b.Description LIKE '%' + @Description + '%')
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
                
                    -- Paginação dos dados
                    SELECT *
                    FROM FilteredBusinesses b
                    ORDER BY 
                        CASE 
                            WHEN DistanceKm IS NULL THEN b.Name
                            ELSE DistanceKm
                        END
                    OFFSET (@PageNumber - 1) * @PageSize ROWS
                    FETCH NEXT @PageSize ROWS ONLY;
                
                    -- Contagem total para paginação
                    SELECT COUNT(*) AS TotalCount
                    FROM (
                        SELECT 1 Dummy
                        FROM [Businesses].[Business] b
                        INNER JOIN [Common].[Addresses] a ON b.AddressId = a.Id
                        WHERE
                            b.IsActive = 1
                            AND b.EmailConfirmed = 1
                            AND b.Deleted IS NULL
                            AND b.DocumentValidated = 1
                            AND (@Name IS NULL OR b.Name LIKE '%' + @Name + '%')
                            AND (@Description IS NULL OR b.Description LIKE '%' + @Description + '%')
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
            migrationBuilder.Sql(@"BEGIN TRY
                    IF OBJECT_ID('[Businesses].[GetFilteredBusinesses]', 'P') IS NOT NULL
                    BEGIN
                        DROP PROCEDURE [Businesses].[GetFilteredBusinesses];
                        PRINT 'Procedure deletada com sucesso.';
                    END
                    ELSE
                    BEGIN
                        PRINT 'A procedure não existe.';
                    END
                END TRY
                BEGIN CATCH
                    PRINT 'Erro ao tentar deletar a procedure.';
                    PRINT ERROR_MESSAGE(); -- Exibe a mensagem de erro
                END CATCH;
            ");
        }
    }
}
