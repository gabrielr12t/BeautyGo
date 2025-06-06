﻿using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Logging;
using Polly;

namespace BeautyGo.BackgroundTasks;

public class RabbitMqRetryPolicy : IRabbitMqRetryPolicy
{
    private readonly AsyncPolicy _retryPolicy;
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;

    public RabbitMqRetryPolicy(ILogger logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;

        var retryIntervals = new[]
        {
            TimeSpan.FromSeconds(10),  // 1º Retry  
            TimeSpan.FromMinutes(1),   // 2º Retry  
            TimeSpan.FromMinutes(2),  // 3º Retry    
        };

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(retryIntervals, OnRetryAsync);
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(Func<Task> action) =>
        await _retryPolicy.ExecuteAsync(action);

    private async Task OnRetryAsync(Exception exception, TimeSpan timeSpan, int attempt, Context context)
    {
        await _logger.WarningAsync($"Retry {attempt} after {timeSpan.TotalSeconds}s due to: {exception.Message}", exception);
        await _unitOfWork.SaveChangesAsync();
    }
}
