using BeautyGo.Domain.Core.Configurations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace BeautyGo.Domain.Caching;

public partial class MemoryCacheManager : CacheKeyService, IStaticCacheManager
{
    #region Fields

    protected bool _disposed;

    protected readonly IMemoryCache _memoryCache;

    protected readonly ICacheKeyManager _keyManager;

    protected static CancellationTokenSource _clearToken = new();

    #endregion

    #region Ctor

    public MemoryCacheManager(AppSettings appSettings, IMemoryCache memoryCache, ICacheKeyManager cacheKeyManager)
        : base(appSettings)
    {
        _memoryCache = memoryCache;
        _keyManager = cacheKeyManager;
    }

    #endregion

    #region Utilities

    protected virtual MemoryCacheEntryOptions PrepareEntryOptions(CacheKey key)
    {
        //set expiration time for the passed cache key
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(key.CacheTime)
        };

        //add token to clear cache entries
        options.AddExpirationToken(new CancellationChangeToken(_clearToken.Token));
        options.RegisterPostEvictionCallback(OnEviction);
        _keyManager.AddKey(key.Key);

        return options;
    }

    protected virtual void OnEviction(object key, object value, EvictionReason reason, object state)
    {
        switch (reason)
        {
            // we clean up after ourselves elsewhere
            case EvictionReason.Removed:
            case EvictionReason.Replaced:
            case EvictionReason.TokenExpired:
                break;
            //if the entry was evicted by the cache itself, we remove the key
            default:
                //checks if the eviction callback happens after the item is re-added to the cache to prevent the erroneously removing entry from the key manager
                if (!_memoryCache.TryGetValue(key, out _))
                    _keyManager.RemoveKey(key as string);
                break;
        }
    }

    #endregion

    #region Methods

    public Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        var key = PrepareKey(cacheKey, cacheKeyParameters).Key;
        _memoryCache.Remove(key);
        _keyManager.RemoveKey(key);

        return Task.CompletedTask;
    }

    public async Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> acquire)
    {
        if ((key?.CacheTime ?? 0) <= 0)
            return await acquire();

        var task = _memoryCache.GetOrCreate(
            key.Key,
            entry =>
            {
                entry.SetOptions(PrepareEntryOptions(key));
                return new Lazy<Task<T>>(acquire, true);
            });

        try
        {
            var data = await task!.Value;

            //if a cached function return null, remove it from the cache
            if (data == null)
                await RemoveAsync(key);

            return data;
        }
        catch (Exception ex)
        {
            //if a cached function throws an exception, remove it from the cache
            await RemoveAsync(key);

            if (ex is NullReferenceException)
                return default;

            throw;
        }
    }

    public async Task<T> GetAsync<T>(CacheKey key, T defaultValue = default)
    {
        var value = _memoryCache.Get<Lazy<Task<T>>>(key.Key)?.Value;

        try
        {
            return value != null ? await value : defaultValue;
        }
        catch
        {
            //if a cached function throws an exception, remove it from the cache
            await RemoveAsync(key);

            throw;
        }
    }

    public async Task<T> GetAsync<T>(CacheKey key, Func<T> acquire)
    {
        return await GetAsync(key, () => Task.FromResult(acquire()));
    }

    public async Task<object> GetAsync(CacheKey key)
    {
        var entry = _memoryCache.Get(key.Key);
        if (entry == null)
            return null;
        try
        {
            if (entry.GetType().GetProperty("Value")?.GetValue(entry) is not Task task)
                return null;

            await task;

            return task.GetType().GetProperty("Result")!.GetValue(task);
        }
        catch
        {
            //if a cached function throws an exception, remove it from the cache
            await RemoveAsync(key);

            throw;
        }
    }

    public Task SetAsync<T>(CacheKey key, T data)
    {
        if (data != null && (key?.CacheTime ?? 0) > 0)
            _memoryCache.Set(
                key.Key,
                new Lazy<Task<T>>(() => Task.FromResult(data), true),
                PrepareEntryOptions(key));

        return Task.CompletedTask;
    }

    public Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters)
    {
        foreach (var key in _keyManager.RemoveByPrefix(PrepareKeyPrefix(prefix, prefixParameters)))
            _memoryCache.Remove(key);

        return Task.CompletedTask;
    }

    public Task ClearAsync()
    {
        _clearToken.Cancel();
        _clearToken.Dispose();
        _clearToken = new CancellationTokenSource();
        _keyManager.Clear();

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
            _clearToken.Dispose();

        _disposed = true;
    }

    #endregion
}
