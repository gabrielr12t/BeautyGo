using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyGo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateCalculateDistance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
            migrationBuilder.Sql(@"
                         CREATE PROCEDURE dbo.GetAddressesWithinRadiusPaged
                            @UserLatitude FLOAT,
                            @UserLongitude FLOAT,
                            @MaxDistanceKm FLOAT,
                            @PageNumber INT,
                            @PageSize INT
                        AS
                        BEGIN
                            SET NOCOUNT ON;
                        
                            -- Declarando variável para armazenar o total de registros encontrados
                            DECLARE @TotalRecords INT;
                        
                            -- CTE para calcular a distância e filtrar os endereços dentro do raio
                            WITH AddressesInRadius AS (
                                SELECT 
                                    a.Id,
                                    a.FirstName,
                                    a.LastName,
                                    a.City,
                                    a.State,
                                    a.StateAbbreviation,
                                    a.Neighborhood,
                                    a.Number,
                                    a.Street,
                                    a.PostalCode,
                                    a.PhoneNumber,
                                    a.Latitude,
                                    a.Longitude,
                                    a.CreatedOn
                                    -- Cálculo de distância utilizando a fórmula de Haversine
                                    (6371 * ACOS(
                                        COS(RADIANS(@UserLatitude)) * COS(RADIANS(a.Latitude)) *
                                        COS(RADIANS(a.Longitude) - RADIANS(@UserLongitude)) +
                                        SIN(RADIANS(@UserLatitude)) * SIN(RADIANS(a.Latitude))
                                    )) AS DistanceKm
                                FROM [BeautyGo-staging].[Common].[Addresses] a
                                WHERE 
                                    -- Filtrar os endereços dentro do raio especificado
                                    (6371 * ACOS(
                                        COS(RADIANS(@UserLatitude)) * COS(RADIANS(a.Latitude)) *
                                        COS(RADIANS(a.Longitude) - RADIANS(@UserLongitude)) +
                                        SIN(RADIANS(@UserLatitude)) * SIN(RADIANS(a.Latitude))
                                    )) <= @MaxDistanceKm
                            )
                        
                            -- Contar o total de registros dentro do raio antes da paginação
                            
                        
                            -- Retornando os endereços paginados
                            SELECT 
                                Id, FirstName, LastName, City, State, StateAbbreviation,
                                Neighborhood, Number, Street, PostalCode, PhoneNumber, 
                                Latitude, Longitude, DistanceKm
                            FROM AddressesInRadius
                            ORDER BY DistanceKm
                            OFFSET (@PageNumber - 1) * @PageSize ROWS
                            FETCH NEXT @PageSize ROWS ONLY;
                        	 
                        END;

            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"BEGIN TRY
                    IF OBJECT_ID('dbo.GetAddressesWithinRadius', 'P') IS NOT NULL
                    BEGIN
                        DROP PROCEDURE dbo.GetAddressesWithinRadius;
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
