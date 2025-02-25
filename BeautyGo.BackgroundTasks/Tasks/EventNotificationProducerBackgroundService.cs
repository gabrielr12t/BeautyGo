using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Logging;
using BeautyGo.BackgroundTasks.Services.Events;
using BeautyGo.BackgroundTasks.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BeautyGo.BackgroundTasks.Tasks;

internal class EventNotificationProducerBackgroundService : BackgroundService
{
    #region Fields

    private readonly BackgroundTaskSettings _backgroundTaskSettings;
    private readonly IServiceProvider _serviceProvider;

    #endregion

    #region Ctor

    public EventNotificationProducerBackgroundService(
       IOptions<BackgroundTaskSettings> backgroundTaskSettingsOptions,
       IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _backgroundTaskSettings = backgroundTaskSettingsOptions.Value;
    }

    #endregion

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        await logger.InformationAsync($"{nameof(EventNotificationProducerBackgroundService)} is starting.");

        stoppingToken.Register(() => logger.InformationAsync($"{nameof(EventNotificationProducerBackgroundService)} background task is stopping."));

        while (!stoppingToken.IsCancellationRequested)
        {
            await logger.InformationAsync($"{nameof(EventNotificationProducerBackgroundService)} background task is doing background work.");

            await ProduceEventNotificationsAsync(stoppingToken);

            await unitOfWork.SaveChangesAsync(stoppingToken);

            await Task.Delay(_backgroundTaskSettings.SleepTimeInMilliseconds, stoppingToken);
        }

        //await logger.InformationAsync($"{nameof(EventNotificationProducerBackgroundService)} background task is stopping.");

        await Task.CompletedTask;
    }

    private async Task ProduceEventNotificationsAsync(CancellationToken stoppingToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();

        try
        {
            var personalEventNotificationsProducer = scope.ServiceProvider.GetRequiredService<IEventNotificationProducer>();

            await personalEventNotificationsProducer.ProduceAsync(_backgroundTaskSettings.NotificationsBatchSize, stoppingToken);
        }
        catch (Exception e)
        {
            await logger.ErrorAsync($"ERROR: Failed to process the batch of events: {e.Message}", e);
        }
    }
}
