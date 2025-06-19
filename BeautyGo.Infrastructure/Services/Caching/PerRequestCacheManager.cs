using BeautyGo.Application.Core.Abstractions.Caching;
using BeautyGo.Domain.Caching;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Core.Lists;

namespace BeautyGo.Infrastructure.Services.Caching;

public partial class PerRequestCacheManager : CacheKeyService, IShortTermCacheManager
{
    #region Fields

    protected readonly ConcurrentTrie<object> _concurrentCollection;

    #endregion

    #region Ctor

    public PerRequestCacheManager(AppSettings appSettings) : base(appSettings)
    {
        _concurrentCollection = new ConcurrentTrie<object>();
    }

    #endregion

    #region Methods

    public async Task<T> GetAsync<T>(Func<Task<T>> acquire, CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        var key = cacheKey.Create(CreateCacheKeyParameters, cacheKeyParameters).Key;

        if (_concurrentCollection.TryGetValue(key, out var data))
            return (T)data;

        var result = await acquire();

        if (result != null)
            _concurrentCollection.Add(key, result);

        return result;
    }

    public virtual void RemoveByPrefix(string prefix, params object[] prefixParameters)
    {
        var keyPrefix = PrepareKeyPrefix(prefix, prefixParameters);
        _concurrentCollection.Prune(keyPrefix, out _);
    }

    public virtual void Remove(string cacheKey, params object[] cacheKeyParameters)
    {
        _concurrentCollection.Remove(PrepareKey(new CacheKey(cacheKey), cacheKeyParameters).Key);
    }

    #endregion
}
