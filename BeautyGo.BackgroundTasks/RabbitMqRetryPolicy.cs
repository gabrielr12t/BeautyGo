﻿using BeautyGo.Application.Core.Abstractions.Logging;
using Polly;

namespace BeautyGo.BackgroundTasks;

public class RabbitMqRetryPolicy
{
    private readonly AsyncPolicy _retryPolicy;
    private readonly ILogger _logger;

    public RabbitMqRetryPolicy(ILogger logger = null)
    {
        _logger = logger;

        var retryIntervals = new[]
        {
            TimeSpan.FromSeconds(10),  // 1º Retry  
            TimeSpan.FromMinutes(1),   // 2º Retry  
            TimeSpan.FromMinutes(10),  // 3º Retry  
            TimeSpan.FromHours(2),     // 4º Retry  
            TimeSpan.FromDays(4)       // 5º Retry  
        };

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(retryIntervals, OnRetryAsync);
    }

    public async Task ExecuteAsync(Func<Task> action) =>
        await _retryPolicy.ExecuteAsync(action);

    private async Task OnRetryAsync(Exception exception, TimeSpan timeSpan, int attempt, Context context) =>
        await _logger.WarningAsync($"Retry {attempt} after {timeSpan.TotalSeconds}s due to: {exception.Message}", exception);
}
