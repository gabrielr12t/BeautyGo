using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Logging;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Settings;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace BeautyGo.Infrastructure.Messaging;

internal sealed class RabbitMqBusEvent : IPublisherBusEvent, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly MessageBrokerSettings _messageBrokerSettings;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqBusEvent(AppSettings appSettings, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _messageBrokerSettings = appSettings.Get<MessageBrokerSettings>();

        var connectionFactory = new ConnectionFactory
        {
            HostName = _messageBrokerSettings.HostName,
            Port = _messageBrokerSettings.Port,
            UserName = _messageBrokerSettings.UserName,
            Password = _messageBrokerSettings.Password,
            DispatchConsumersAsync = true
        };

        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

        // Garantir que a fila exista antes de publicar
        _channel.QueueDeclare(_messageBrokerSettings.QueueName, durable: true, exclusive: false, autoDelete: false);
    }

    public async Task PublishAsync(IBusEvent @event, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();

        try
        {
            var payload = JsonConvert.SerializeObject(@event, typeof(IBusEvent), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
            });

            var body = Encoding.UTF8.GetBytes(payload);
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true; // Mensagem persistente

            await logger.InformationAsync($"MESSAGE: {@event.GetType()} - Preparing to publish - {@event}");

            _channel.BasicPublish(
                exchange: string.Empty,
                routingKey: _messageBrokerSettings.QueueName,
                basicProperties: properties,
                body: body
            );

            await logger.InformationAsync($"MESSAGE: {@event.GetType()} - Published - {@event}");
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync($"MESSAGE: {@event.GetType()} - Publish failed - {ex.Message}", ex);
            throw;
        }
    }

    public void Dispose()
    {
        if (_channel?.IsOpen == true)
        {
            _channel.Close();
            _channel.Dispose();
        }

        if (_connection?.IsOpen == true)
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
