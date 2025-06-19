using BeautyGo.Domain.Caching;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Core.Lists;
using Microsoft.Extensions.Caching.Distributed;

namespace BeautyGo.Infrastructure.Services.Caching;

public partial class MemoryDistributedCacheManager : DistributedCacheManager
{
    #region Ctor

    public MemoryDistributedCacheManager(AppSettings appSettings,
        IDistributedCache distributedCache,
        ICacheKeyManager cacheKeyManager,
        IConcurrentCollection<object> concurrentCollection)
        : base(appSettings, distributedCache, cacheKeyManager, concurrentCollection)
    {
    }

    #endregion

    public override async Task ClearAsync()
    {
        foreach (var key in _localKeyManager.Keys)
            await RemoveAsync(key, false);

        ClearInstanceData();
    }

    public override async Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters)
    {
        var keyPrefix = PrepareKeyPrefix(prefix, prefixParameters);

        foreach (var key in RemoveByPrefixInstanceData(keyPrefix))
            await RemoveAsync(key, false);
    }
}
