using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Logging;
using BeautyGo.BackgroundTasks.Services.Emails;
using BeautyGo.BackgroundTasks.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Data.SqlTypes;

namespace BeautyGo.BackgroundTasks.Tasks;

internal class EmailNotificationConsumerBackgroundService : BackgroundService
{
    private readonly BackgroundTaskSettings _backgroundTaskSettings;
    private readonly IServiceProvider _serviceProvider;

    public EmailNotificationConsumerBackgroundService(
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

        await logger.InformationAsync($"{nameof(EmailNotificationConsumerBackgroundService)} is starting.");

        stoppingToken.Register(() => logger.InformationAsync("EmailNotificationConsumerBackgroundService background task is stopping."));

        while (!stoppingToken.IsCancellationRequested)
        {
            //await logger.InformationAsync("EmailNotificationConsumerBackgroundService background task is doing background work.");

            await ConsumeEventNotificationsAsync(stoppingToken);

            await unitOfWork.SaveChangesAsync(stoppingToken);

            await Task.Delay(_backgroundTaskSettings.SleepTimeInMilliseconds, stoppingToken);
        }

        //await logger.InformationAsync("EmailNotificationConsumerBackgroundService background task is stopping.");

        await Task.CompletedTask;
    }

    private async Task ConsumeEventNotificationsAsync(CancellationToken stoppingToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        try
        {
            var emailNotificationsConsumer = scope.ServiceProvider.GetRequiredService<IEmailNotificationsConsumer>();

            await emailNotificationsConsumer.ConsumeAsync(
                _backgroundTaskSettings.NotificationsBatchSize, 
                stoppingToken);
        }
        catch (Exception e)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger>();

            await logger.ErrorAsync($"ERROR: Failed to process the batch of notifications: {e.Message}", e);
        }
        finally
        {
            await unitOfWork.SaveChangesAsync();
        }
    }
}
