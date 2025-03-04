using BeautyGo.BackgroundTasks.Services.Emails;
using BeautyGo.BackgroundTasks.Services.Events;
using BeautyGo.BackgroundTasks.Services.Integrations;
using BeautyGo.BackgroundTasks.Services.OutboxMessages;
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
        services.AddHostedService<BusEventConsumerBackgroundService>();
        services.AddHostedService<EventProcessorBackgroundService>();
        services.AddHostedService<ProcessOutboxMessagesProducerBackgroundService>();

        services.AddScoped<IEmailNotificationsConsumer, EmailNotificationsConsumer>();
        services.AddScoped<IBusEventConsumer, BusEventConsumer>();
        services.AddScoped<IEventProcessor, EventProcessor>();
        services.AddScoped<IProcessOutboxMessagesProducer, ProcessOutboxMessagesProducer>();

        services.AddScoped<IRabbitMqRetryPolicy, RabbitMqRetryPolicy>();

        return services;
    }
}
