using BeautyGo.Application.Core.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace BeautyGo.Infrastructure.Services.Caching;

public partial class DistributedCacheLocker : ILocker
{
    #region Fields

    protected static readonly string _running = JsonConvert.SerializeObject(TaskStatus.Running);
    protected readonly IDistributedCache _distributedCache;

    #endregion

    #region Ctor

    public DistributedCacheLocker(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    #endregion

    #region Methods

    public async Task<bool> PerformActionWithLockAsync(string resource, TimeSpan expirationTime, Func<Task> action)
    {
        //ensure that lock is acquired
        if (!string.IsNullOrEmpty(await _distributedCache.GetStringAsync(resource)))
            return false;

        try
        {
            await _distributedCache.SetStringAsync(resource, resource, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expirationTime
            });

            await action();

            return true;
        }
        finally
        {
            //release lock even if action fails
            await _distributedCache.RemoveAsync(resource);
        }
    }

    public async Task RunWithHeartbeatAsync(string key, TimeSpan expirationTime, TimeSpan heartbeatInterval, Func<CancellationToken, Task> action, CancellationTokenSource cancellationTokenSource = default)
    {
        if (!string.IsNullOrEmpty(await _distributedCache.GetStringAsync(key)))
            return;

        var tokenSource = cancellationTokenSource ?? new CancellationTokenSource();

        try
        {
            // run heartbeat early to minimize risk of multiple execution
            await _distributedCache.SetStringAsync(
                key,
                _running,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expirationTime },
                token: tokenSource.Token);

            await using var timer = new Timer(
                callback: _ =>
                {
                    try
                    {
                        tokenSource.Token.ThrowIfCancellationRequested();
                        var status = _distributedCache.GetString(key);
                        if (!string.IsNullOrEmpty(status) && JsonConvert.DeserializeObject<TaskStatus>(status) ==
                            TaskStatus.Canceled)
                        {
                            tokenSource.Cancel();
                            return;
                        }

                        _distributedCache.SetString(
                            key,
                            _running,
                            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expirationTime });
                    }
                    catch (OperationCanceledException) { }
                },
                state: null,
                dueTime: 0,
                period: (int)heartbeatInterval.TotalMilliseconds);

            await action(tokenSource.Token);
        }
        catch (OperationCanceledException) { }
        finally
        {
            await _distributedCache.RemoveAsync(key);
        }
    }

    public async Task CancelTaskAsync(string key, TimeSpan expirationTime)
    {
        var status = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(status) &&
            JsonConvert.DeserializeObject<TaskStatus>(status) != TaskStatus.Canceled)
            await _distributedCache.SetStringAsync(
                key,
                JsonConvert.SerializeObject(TaskStatus.Canceled),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expirationTime });
    }

    public async Task<bool> IsTaskRunningAsync(string key)
    {
        return !string.IsNullOrEmpty(await _distributedCache.GetStringAsync(key));
    }

    #endregion
}
