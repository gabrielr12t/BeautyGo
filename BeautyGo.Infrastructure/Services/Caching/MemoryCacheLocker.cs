using BeautyGo.Application.Core.Abstractions.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace BeautyGo.Infrastructure.Services.Caching;

public partial class MemoryCacheLocker : ILocker
{
    #region Fields

    protected readonly IMemoryCache _memoryCache;

    #endregion

    #region Ctor

    public MemoryCacheLocker(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    #endregion

    #region Utilities

    protected virtual async Task<bool> RunAsync(string key, TimeSpan? expirationTime, Func<CancellationToken, Task> action, CancellationTokenSource cancellationTokenSource = default)
    {
        var started = false;

        try
        {
            var tokenSource = _memoryCache.GetOrCreate(key, entry => new Lazy<CancellationTokenSource>(() =>
            {
                entry.AbsoluteExpirationRelativeToNow = expirationTime;
                entry.SetPriority(CacheItemPriority.NeverRemove);
                started = true;
                return cancellationTokenSource ?? new CancellationTokenSource();
            }, true))?.Value;

            if (tokenSource != null && started)
                await action(tokenSource.Token);
        }
        catch (OperationCanceledException) { }
        finally
        {
            if (started)
                _memoryCache.Remove(key);
        }

        return started;
    }

    #endregion

    #region Methods

    public async Task<bool> PerformActionWithLockAsync(string resource, TimeSpan expirationTime, Func<Task> action)
    {
        return await RunAsync(resource, expirationTime, _ => action());
    }

    public async Task RunWithHeartbeatAsync(string key, TimeSpan expirationTime, TimeSpan heartbeatInterval, Func<CancellationToken, Task> action, CancellationTokenSource cancellationTokenSource = default)
    {
        await RunAsync(key, null, action, cancellationTokenSource);
    }

    public Task CancelTaskAsync(string key, TimeSpan expirationTime)
    {
        if (_memoryCache.TryGetValue(key, out Lazy<CancellationTokenSource> tokenSource))
            tokenSource.Value.Cancel();

        return Task.CompletedTask;
    }

    public Task<bool> IsTaskRunningAsync(string key)
    {
        return Task.FromResult(_memoryCache.TryGetValue(key, out _));
    }

    #endregion
}
