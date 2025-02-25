using BeautyGo.BackgroundTasks.Services.Emails;
using BeautyGo.BackgroundTasks.Services.Events;
using BeautyGo.BackgroundTasks.Services.Integrations;
using BeautyGo.BackgroundTasks.Settings;
using BeautyGo.BackgroundTasks.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BeautyGo.BackgroundTasks;

public static class DependencyInjection
{
    public static IServiceCollection AddBackgroundTasks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.Configure<BackgroundTaskSettings>(configuration.GetSection(BackgroundTaskSettings.SettingsKey));

        services.AddHostedService<EmailNotificationConsumerBackgroundService>();
        services.AddHostedService<IntegrationEventConsumerBackgroundService>();
        services.AddHostedService<EventNotificationProducerBackgroundService>();

        services.AddScoped<IEmailNotificationsConsumer, EmailNotificationsConsumer>();
        services.AddScoped<IIntegrationEventConsumer, IntegrationEventConsumer>();
        services.AddScoped<IEventNotificationProducer, EventNotificationProducer>();

        return services;
    }
}
