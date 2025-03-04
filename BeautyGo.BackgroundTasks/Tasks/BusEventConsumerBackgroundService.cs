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

internal sealed class BusEventConsumerBackgroundService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly MessageBrokerSettings _settings;

    public BusEventConsumerBackgroundService(AppSettings appSettings, IServiceProvider serviceProvider)
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

        _channel.QueueDeclare(_settings.QueueName, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueDeclare(_settings.DLQName, durable: true, exclusive: false, autoDelete: false);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += OnIntegrationEventReceived;
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
        if (_channel?.IsOpen == true)
        {
            _channel.Close();
        }

        if (_connection?.IsOpen == true)
        {
            _connection.Close();
        }
    }

    private async Task OnIntegrationEventReceived(object sender, BasicDeliverEventArgs eventArgs)
    {
        using var scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        IBusEvent? @event = null;

        try
        {
            string body = Encoding.UTF8.GetString(eventArgs.Body.Span);

            @event = JsonConvert.DeserializeObject<IBusEvent>(body, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            await logger.InformationAsync($"MESSAGE: {@event?.GetType()} - Received - {@event}");

            await ProcessIntegrationEventAsync(scope.ServiceProvider, @event);

            await unitOfWork.SaveChangesAsync(); // Salvar antes de dar BasicAck

            _channel.BasicAck(eventArgs.DeliveryTag, false);

            await logger.InformationAsync($"MESSAGE: {@event?.GetType()} - Processed - {@event}");
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync($"MESSAGE: {@event?.GetType()} - Error - {ex.Message} -{@event}", ex);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.Headers = eventArgs.BasicProperties.Headers; // Manter headers

            _channel.BasicNack(eventArgs.DeliveryTag, false, false);
            _channel.BasicPublish("", _settings.DLQName, properties, eventArgs.Body.ToArray());
        }
    }

    private async Task ProcessIntegrationEventAsync(IServiceProvider serviceProvider, IBusEvent integrationEvent)
    {
        var eventConsumer = serviceProvider.GetRequiredService<IBusEventConsumer>();

        var retryPolicy = serviceProvider.GetRequiredService<IRabbitMqRetryPolicy>();
        await retryPolicy.ExecuteAsync(() => eventConsumer.ConsumeAsync(integrationEvent));
    }
}


