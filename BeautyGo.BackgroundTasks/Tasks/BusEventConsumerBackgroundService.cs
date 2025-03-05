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
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly MessageBrokerSettings _settings;

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

        _channel.QueueDeclare(_settings.QueueName, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueDeclare(_settings.DLQName, durable: true, exclusive: false, autoDelete: false);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (sender, eventArgs) => await HandleMessageAsync(eventArgs);

        _channel.BasicConsume(queue: _settings.QueueName, autoAck: false, consumer: consumer);

        await Task.CompletedTask;
    }

    private async Task HandleMessageAsync(BasicDeliverEventArgs eventArgs)
    {
        using var scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var eventConsumer = scope.ServiceProvider.GetRequiredService<IBusEventConsumer>();
        var retryPolicy = scope.ServiceProvider.GetRequiredService<IRabbitMqRetryPolicy>();

        IBusEvent? @event = null;

        try
        {
            var body = Encoding.UTF8.GetString(eventArgs.Body.Span);
            @event = JsonConvert.DeserializeObject<IBusEvent>(body, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            if (@event == null)
                throw new Exception("Mensagem inválida ou não reconhecida.");

            await logger.InformationAsync($"MESSAGE: {@event.GetType()} - Received - {@event}");

            await retryPolicy.ExecuteAsync(() => eventConsumer.ConsumeAsync(@event));

            await unitOfWork.SaveChangesAsync();

            _channel.BasicAck(eventArgs.DeliveryTag, false);

            await logger.InformationAsync($"MESSAGE: {@event.GetType()} - Processed - {@event}");
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync($"MESSAGE: {@event?.GetType()} - Error - {ex.Message} -{@event}", ex);

            HandleFailedMessage(eventArgs);
        }
    }

    private void HandleFailedMessage(BasicDeliverEventArgs eventArgs)
    {
        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.Headers = eventArgs.BasicProperties.Headers;

        _channel.BasicPublish(exchange: "", routingKey: _settings.DLQName, basicProperties: properties, body: eventArgs.Body.ToArray());

        _channel.BasicAck(eventArgs.DeliveryTag, false);
    }

    public override void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();

        base.Dispose();
    }
}



