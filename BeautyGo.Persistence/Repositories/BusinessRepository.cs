using BeautyGo.Contracts.Businesses;
using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Core.Infrastructure;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Repositories;
using BeautyGo.Persistence.Repositories.Bases;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BeautyGo.Persistence.Repositories;

internal class BusinessRepository : EFBaseRepository<Business>, IBusinessRepository
{
    public BusinessRepository(BeautyGoContext context) : base(context)
    {
    }

    public async Task<IPagedList<BusinessFilterModel>> FilterAsync(BusinessFilter filter, CancellationToken cancellation = default)
    {
        using var connection = new SqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync(cancellation);

        var parameters = new DynamicParameters();
        parameters.Add($"@{nameof(BusinessFilter.Latitude)}", filter.Latitude);
        parameters.Add($"@{nameof(BusinessFilter.Longitude)}", filter.Longitude);
        parameters.Add($"@{nameof(BusinessFilter.RadiusKm)}", filter.RadiusKm);
        parameters.Add($"@{nameof(BusinessFilter.Name)}", filter.Name);
        parameters.Add($"@{nameof(BusinessFilter.Description)}", filter.Description);
        parameters.Add($"@{nameof(BusinessFilter.PageNumber)}", filter.PageNumber);
        parameters.Add($"@{nameof(BusinessFilter.PageSize)}", filter.PageSize);

        using var multi = await connection.QueryMultipleAsync(
            "Businesses.GetFilteredBusinesses",
            parameters, commandType: CommandType.StoredProcedure);

        var items = (await multi.ReadAsync<BusinessFilterModel>()).ToList();
        var totalCount = await multi.ReadFirstOrDefaultAsync<int>();

        return new PagedList<BusinessFilterModel>(items, filter.PageNumber, filter.PageSize, totalCount);
    }
}
