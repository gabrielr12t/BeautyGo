using BeautyGo.Contracts.Address;
using BeautyGo.Domain.Entities.Common;
using BeautyGo.Domain.Repositories;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BeautyGo.Persistence.Repositories;

internal sealed class AddressRepository : BaseRepository<Address>, IAddressRepository
{
    public AddressRepository(BeautyGoContext context) : base(context) { }

    public async Task<IList<AddressNearbyDTO>> GetAddressWithinRadiusAsync(double latitude, double longitude, double maxRadiousKm, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        const string query = "EXEC dbo.GetAddressesWithinRadiusPaged @Latitude, @Longitude, @MaxDistanceKm, @PageNumber, @PageSize";

        var parameters = new
        {
            Latitude = latitude,
            Longitude = longitude,
            MaxDistanceKm = maxRadiousKm,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        using var connection = new SqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync(cancellationToken);

        var addresses = await connection.QueryAsync<AddressNearbyDTO>(query, parameters, commandType: CommandType.Text);

        return addresses.ToList();
    }
}
