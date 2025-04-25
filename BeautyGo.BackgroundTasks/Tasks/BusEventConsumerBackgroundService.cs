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

internal sealed class BusEventConsumerBackgroundService : BackgroundService, IDisposable
{
    #region Fields

    private readonly IServiceProvider _serviceProvider;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly MessageBrokerSettings _settings;

    #endregion

    #region Ctor

    public BusEventConsumerBackgroundService(
        AppSettings appSettings,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _settings = appSettings.Get<MessageBrokerSettings>() ?? throw new ArgumentNullException(nameof(appSettings));

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

        QueueDeclares();
        ConfigureExchange();
        QueueBind();
    }

    #endregion

    #region Utilities

    private void QueueDeclares()
    {
        // Garantir que as filas existam
        _channel.QueueDeclare(
            queue: _settings.QueueName,
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
            queue: _settings.RetryQueueName,
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
            queue: _settings.DLQName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: dlqArguments
        );
    }

    private void ConfigureExchange()
    {
        // Configurar Exchange
        _channel.ExchangeDeclare(
            exchange: _settings.ExchangeName,
            type: "direct", // Pode ser "direct", "fanout", etc.
            durable: true
        );
    }

    private void QueueBind()
    {
        // Associar filas ao Exchange
        _channel.QueueBind(
            queue: _settings.QueueName,
            exchange: _settings.ExchangeName,
            routingKey: _settings.QueueName
        );

        _channel.QueueBind(
            queue: _settings.RetryQueueName,
            exchange: _settings.ExchangeName,
            routingKey: _settings.RetryQueueName
        );

        _channel.QueueBind(
            queue: _settings.DLQName,
            exchange: _settings.ExchangeName,
            routingKey: _settings.DLQName
        );
    }

    private async Task HandleMessageAsync(BasicDeliverEventArgs eventArgs, CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var eventConsumer = scope.ServiceProvider.GetRequiredService<IBusEventConsumer>();
        var retryPolicy = scope.ServiceProvider.GetRequiredService<IRabbitMqRetryPolicy>();

        IIntegrationEvent? @event = null;

        try
        {
            var body = Encoding.UTF8.GetString(eventArgs.Body.Span);
            @event = JsonConvert.DeserializeObject<IIntegrationEvent>(body, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            if (@event == null)
                throw new Exception("Mensagem inválida ou não reconhecida.");

            await logger.InformationAsync($"MESSAGE: {@event.GetType()} - Received - {@event}");

            await retryPolicy.ExecuteAsync(() => eventConsumer.ConsumeAsync(@event, stoppingToken));

            await unitOfWork.SaveChangesAsync();

            _channel.BasicAck(eventArgs.DeliveryTag, false);

            await logger.InformationAsync($"MESSAGE: {@event.GetType()} - Processed - {@event}");
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync($"MESSAGE: {@event?.GetType()} - Error - {ex.Message} -{@event}", ex);

            // Redireciona para DLQ ou fila de retry
            await HandleFailedMessageAsync(eventArgs);
        }
    }

    private Task HandleFailedMessageAsync(BasicDeliverEventArgs eventArgs)
    {
        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.Headers = eventArgs.BasicProperties.Headers;

        // Publica na fila de DLQ
        _channel.BasicPublish(exchange: "", routingKey: _settings.DLQName, basicProperties: properties, body: eventArgs.Body.ToArray());

        _channel.BasicAck(eventArgs.DeliveryTag, false);

        return Task.CompletedTask;
    }

    #endregion

    #region Execute

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (sender, eventArgs) => await HandleMessageAsync(eventArgs, stoppingToken);

        _channel.BasicConsume(queue: _settings.QueueName, autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }

    #endregion

    #region Dispose

    public override void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();

        base.Dispose();
    }

    #endregion
}




