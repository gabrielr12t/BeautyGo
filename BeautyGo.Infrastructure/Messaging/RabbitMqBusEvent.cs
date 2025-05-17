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
    #region Fields

    private readonly IServiceProvider _serviceProvider;
    private readonly MessageBrokerSettings _messageBrokerSettings;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    #endregion

    #region Ctor

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

        DeclareQueues();
        ConfigureExchange();
        BindQueues();
    }

    #endregion

    #region Utilities

    private void DeclareQueues()
    {
        _channel.QueueDeclare(
            queue: _messageBrokerSettings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        // Configurar fila de Retry com TTL
        var retryQueueArguments = new Dictionary<string, object>
        {
            { "x-message-ttl", 60000 } // 1 minuto de TTL para retry
        };

        _channel.QueueDeclare(
            queue: _messageBrokerSettings.RetryQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: retryQueueArguments
        );

        // Configurar fila de DLQ com TTL
        var dlqArguments = new Dictionary<string, object>
        {
            { "x-message-ttl", 60000 } // 1 minuto de TTL para DLQ
        };

        _channel.QueueDeclare(
            queue: _messageBrokerSettings.DLQName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: dlqArguments
        );
    }

    private void ConfigureExchange()
    {
        _channel.ExchangeDeclare(
            exchange: _messageBrokerSettings.ExchangeName,
            type: "direct",  
            durable: true
        );
    }

    private void BindQueues()
    {
        _channel.QueueBind(
           queue: _messageBrokerSettings.QueueName,
           exchange: _messageBrokerSettings.ExchangeName,
           routingKey: _messageBrokerSettings.QueueName
       );

        _channel.QueueBind(
            queue: _messageBrokerSettings.RetryQueueName,
            exchange: _messageBrokerSettings.ExchangeName,
            routingKey: _messageBrokerSettings.RetryQueueName
        );

        _channel.QueueBind(
            queue: _messageBrokerSettings.DLQName,
            exchange: _messageBrokerSettings.ExchangeName,
            routingKey: _messageBrokerSettings.DLQName
        );
    }

    #endregion

    #region Publish

    public async Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();

        try
        {
            var payload = JsonConvert.SerializeObject(@event, typeof(IIntegrationEvent), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
            });

            var body = Encoding.UTF8.GetBytes(payload);
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true; // Mensagem persistente

            await logger.InformationAsync($"MESSAGE: {@event.GetType()} - Preparing to publish - {@event}");

            _channel.BasicPublish(
                exchange: _messageBrokerSettings.ExchangeName,
                routingKey: _messageBrokerSettings.QueueName, // Usando a fila principal
                basicProperties: properties,
                body: body
            );

            await logger.InformationAsync($"MESSAGE: {@event.GetType()} - Published - {@event}", cancellation: cancellationToken);
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync($"MESSAGE: {@event.GetType()} - Publish failed - {ex.Message}", ex, cancellation: cancellationToken);
            throw;
        }
    }

    #endregion

    #region Dispose

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

    #endregion
}

