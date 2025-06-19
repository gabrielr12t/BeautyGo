using BeautyGo.Domain.Caching;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Core.Lists;
using BeautyGo.Domain.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Distributed;
using System.Data;

namespace BeautyGo.Infrastructure.Services.Caching;

public partial class MsSqlServerCacheManager : DistributedCacheManager
{
    #region Fields

    protected readonly DistributedCacheSettings _distributedCacheConfig;

    #endregion

    #region Ctor

    public MsSqlServerCacheManager(AppSettings appSettings,
        IDistributedCache distributedCache,
        ICacheKeyManager cacheKeyManager,
        IConcurrentCollection<object> concurrentCollection)
        : base(appSettings, distributedCache, cacheKeyManager, concurrentCollection)
    {
        _distributedCacheConfig = appSettings.Get<DistributedCacheSettings>();
    }

    #endregion

    #region Utilities

    protected virtual async Task PerformActionAsync(SqlCommand command, params SqlParameter[] parameters)
    {
        var conn = new SqlConnection(_distributedCacheConfig.ConnectionString);

        try
        {
            await conn.OpenAsync();
            command.Connection = conn;
            if (parameters.Any())
                command.Parameters.AddRange(parameters);

            await command.ExecuteNonQueryAsync();
        }
        finally
        {
            await conn.CloseAsync();
        }
    }

    #endregion

    #region Methods

    public override async Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters)
    {
        prefix = PrepareKeyPrefix(prefix, prefixParameters);

        var command =
            new SqlCommand(
                $"DELETE FROM {_distributedCacheConfig.SchemaName}.{_distributedCacheConfig.TableName} WHERE Id LIKE @Prefix + '%'");

        await PerformActionAsync(command, new SqlParameter("Prefix", SqlDbType.NVarChar) { Value = prefix });

        RemoveByPrefixInstanceData(prefix);
    }

    public override async Task ClearAsync()
    {
        var command =
            new SqlCommand(
                $"TRUNCATE TABLE {_distributedCacheConfig.SchemaName}.{_distributedCacheConfig.TableName}");

        await PerformActionAsync(command);

        ClearInstanceData();
    }

    #endregion
}
