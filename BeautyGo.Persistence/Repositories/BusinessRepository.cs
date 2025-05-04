using BeautyGo.Contracts.Businesses;
using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Core.Infrastructure;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Repositories;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BeautyGo.Persistence.Repositories;

internal class BusinessRepository : BaseRepository<Business>, IBusinessRepository
{
    public BusinessRepository(BeautyGoContext context) : base(context)
    {
    }

    public async Task<IPagedList<BusinessFilterModel>> FilterAsync(BusinessFilter filter, CancellationToken cancellation = default)
    {
        using var connection = new SqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync(cancellation);

        var parameters = new DynamicParameters();
        parameters.Add("@Latitude", filter.Latitude, DbType.Double);
        parameters.Add("@Longitude", filter.Longitude, DbType.Double);
        parameters.Add("@RadiusKm", filter.RadiusKm, DbType.Double);
        parameters.Add("@Name", filter.Name, DbType.String);
        parameters.Add("@Description", filter.Description, DbType.String);
        parameters.Add("@PageNumber", filter.PageNumber, DbType.Int32);
        parameters.Add("@PageSize", filter.PageSize, DbType.Int32);

        using var multi = await connection.QueryMultipleAsync(
            "Businesses.GetFilteredBusinesses",
            parameters, commandType: CommandType.StoredProcedure);

        var items = (await multi.ReadAsync<BusinessFilterModel>()).ToList();
        var totalCount = await multi.ReadFirstOrDefaultAsync<int>();

        return new PagedList<BusinessFilterModel>(items, filter.PageNumber, filter.PageSize, totalCount);
    }
}
