using BeautyGo.BackgroundTasks.Services.Emails;
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

        services.Configure<BackgroundTaskSettings>(p => new BackgroundTaskSettings());

        services.AddHostedService<EmailNotificationConsumerBackgroundService>();
        services.AddHostedService<IntegrationEventConsumerBackgroundService>();

        services.AddScoped<IEmailNotificationsConsumer, EmailNotificationsConsumer>();
        services.AddScoped<IIntegrationEventConsumer, IntegrationEventConsumer>();

        return services;
    }
}
