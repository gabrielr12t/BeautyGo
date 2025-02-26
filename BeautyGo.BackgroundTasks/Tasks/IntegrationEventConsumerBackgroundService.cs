using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Logging;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.BackgroundTasks.Services.Integrations;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BeautyGo.BackgroundTasks.Tasks;

internal sealed class IntegrationEventConsumerBackgroundService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IModel _channel;
    private readonly IConnection _connection;

    private readonly MessageBrokerSettings _settings;

    public IntegrationEventConsumerBackgroundService(AppSettings appSettings, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _settings = appSettings.Get<MessageBrokerSettings>();

        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password,
            DispatchConsumersAsync = true,
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(_settings.QueueName, false, false, false); 
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received +=  OnIntegrationEventReceived;
        _channel.BasicConsume(_settings.QueueName, false, consumer);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Dispose();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }

    private async Task OnIntegrationEventReceived(object sender, BasicDeliverEventArgs eventArgs)
    {
        using var scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        IIntegrationEvent @event = null;

        try
        {
            string body = Encoding.UTF8.GetString(eventArgs.Body.Span);

            var integrationEvent = JsonConvert.DeserializeObject<IIntegrationEvent>(body, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            await logger.InformationAsync($"MESSAGE: {@event.GetType()} - Received - {@event}");

            await ProcessIntegrationEventAsync(scope.ServiceProvider, @event);

            _channel.BasicAck(eventArgs.DeliveryTag, false);

            await logger.InformationAsync($"MESSAGE: {@event.GetType()} - Processed - {@event}");
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync($"MESSAGE: {@event.GetType()} - Error - {ex.Message} -{@event}", ex);
        }
        finally
        {
            await unitOfWork.SaveChangesAsync();
        }
    } 

    private async Task ProcessIntegrationEventAsync(IServiceProvider serviceProvider, IIntegrationEvent integrationEvent)
    {
        var eventConsumer = serviceProvider.GetRequiredService<IIntegrationEventConsumer>();
        await eventConsumer.ConsumeAsync(integrationEvent);
    } 
} 
   
     