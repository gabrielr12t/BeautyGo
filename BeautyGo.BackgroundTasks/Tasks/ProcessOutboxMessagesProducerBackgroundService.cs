using BeautyGo.Application.Common.BackgroundServices;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Logging;
using BeautyGo.BackgroundTasks.Services.OutboxMessages;
using BeautyGo.BackgroundTasks.Settings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BeautyGo.BackgroundTasks.Tasks;

internal class ProcessOutboxMessagesProducerBackgroundService : BackgroundService
{
    private readonly BackgroundTaskSettings _backgroundTaskSettings;
    private readonly IServiceProvider _serviceProvider;

    public ProcessOutboxMessagesProducerBackgroundService(
        IOptions<BackgroundTaskSettings> backgroundTaskSettingsOptions,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _backgroundTaskSettings = backgroundTaskSettingsOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        await logger.InformationAsync($"{nameof(ProcessOutboxMessagesProducerBackgroundService)} is starting.");

        stoppingToken.Register(() => logger.InformationAsync($"{nameof(ProcessOutboxMessagesProducerBackgroundService)} background task is stopping."));

        while (!stoppingToken.IsCancellationRequested)
        {
            //await logger.InformationAsync($"{nameof(ProcessOutboxMessagesProducerBackgroundService)} background task is doing background work.");

            await ProduceEventNotificationsAsync(stoppingToken);

            await unitOfWork.SaveChangesAsync(stoppingToken);

            await Task.Delay(_backgroundTaskSettings.SleepTimeInMilliseconds, stoppingToken);
        }

        await logger.InformationAsync($"{nameof(ProcessOutboxMessagesProducerBackgroundService)} background task is stopping.");
    }

    private async Task ProduceEventNotificationsAsync(CancellationToken stoppingToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        try
        {
            var processesOutboxMessage = scope.ServiceProvider.GetRequiredService<IProcessOutboxMessagesProducer>();

            await processesOutboxMessage.ProduceAsync(stoppingToken);
        }
        catch (Exception e)
        {
            await logger.ErrorAsync($"ERROR: Failed to process the batch of events: {e.Message}", e);

            await unitOfWork.SaveChangesAsync(stoppingToken);
        }
    }
}
