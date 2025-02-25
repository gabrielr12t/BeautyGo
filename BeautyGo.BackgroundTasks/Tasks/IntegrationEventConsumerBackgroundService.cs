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

    public IntegrationEventConsumerBackgroundService(AppSettings appSettings, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        var messageBrokerSettings = appSettings.Get<MessageBrokerSettings>();

        var factory = new ConnectionFactory
        {
            HostName = messageBrokerSettings.HostName,
            Port = messageBrokerSettings.Port,
            UserName = messageBrokerSettings.UserName,
            Password = messageBrokerSettings.Password,
            DispatchConsumersAsync = true,
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        ConfigureQueueWithRetry(messageBrokerSettings.QueueName, "beauty_go_retry_queue", "dlx_exchange");
        StartConsumer(messageBrokerSettings.QueueName);
    }

    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

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

    private void ConfigureQueueWithRetry(string mainQueue, string retryQueue, string deadLetterExchange)
    {
        // Configura a fila principal
        var mainArgs = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", deadLetterExchange }
        };

        _channel.QueueDeclare(mainQueue, true, false, false, mainArgs);

        // Configura a fila de retry com TTL inicial
        var retryArgs = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", string.Empty },
            { "x-dead-letter-routing-key", mainQueue }
        };

        _channel.QueueDeclare(retryQueue, true, false, false, retryArgs);
    }

    private void StartConsumer(string queueName)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += OnIntegrationEventReceived;
        _channel.BasicConsume(queueName, false, consumer);
    }

    private async Task OnIntegrationEventReceived(object sender, BasicDeliverEventArgs eventArgs)
    {
        using var scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        string body = Encoding.UTF8.GetString(eventArgs.Body.Span);
        IIntegrationEvent @event = null;

        try
        {
            @event = DeserializeIntegrationEvent(body);

            //await logger.InformationAsync($"MESSAGE: {message.Id} - Received - {message}");
            await logger.InformationAsync($"MESSAGE: {@event.GetType()} - Received - {@event}");

            await ProcessIntegrationEventAsync(scope.ServiceProvider, @event);

            _channel.BasicAck(eventArgs.DeliveryTag, false);

            await logger.InformationAsync($"MESSAGE: {@event.GetType()} - Processed - {@event}");
        }
        catch (Exception ex)
        {
            // Log do erro
            await logger.ErrorAsync($"MESSAGE: {@event.GetType()} - Error - {ex.Message} -{@event}", ex);

            // Adiciona no retry com atraso exponencial
            await ScheduleRetryAsync(eventArgs, @event);
        }
        finally
        {
            // Salva alterações se necessário
            await unitOfWork.SaveChangesAsync();
        }
    }

    private async Task ScheduleRetryAsync(BasicDeliverEventArgs eventArgs, IIntegrationEvent @event)
    {
        const int maxRetryAttempts = 5;

        var headers = eventArgs.BasicProperties.Headers ?? new Dictionary<string, object>();
        var retryCount = headers.TryGetValue("x-retry-count", out var retryValue)
            ? Convert.ToInt32(retryValue)
            : 0;

        if (retryCount >= maxRetryAttempts)
        {
            // Caso atinja o máximo de tentativas, descarta ou envia para uma Dead Letter
            _channel.BasicReject(eventArgs.DeliveryTag, false);
            return;
        }

        using var scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        await logger.InformationAsync($"MESSAGE: {@event.GetType()} - Retrying {retryCount} - {@event}");

        // Calcula o delay exponencial
        var delay = (int)Math.Pow(3, retryCount) * 1000; // 2^retryCount segundos

        // Configura propriedades da mensagem
        var properties = _channel.CreateBasicProperties();
        properties.Headers = new Dictionary<string, object>
        {
            { "x-retry-count", retryCount + 1 }
        };
        properties.Persistent = true;

        // Publica na fila de retry
        _channel.BasicPublish(
            exchange: string.Empty,
            routingKey: "retry_queue",
            basicProperties: properties,
            body: eventArgs.Body);
    }

    private IIntegrationEvent DeserializeIntegrationEvent(string body)
    {
        return JsonConvert.DeserializeObject<IIntegrationEvent>(body, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        }) ?? throw new InvalidOperationException($"Falha ao desserializar evento: {body}");
    }

    private async Task ProcessIntegrationEventAsync(IServiceProvider serviceProvider, IIntegrationEvent integrationEvent)
    {
        var eventConsumer = serviceProvider.GetRequiredService<IIntegrationEventConsumer>();
        await eventConsumer.ConsumeAsync(integrationEvent);
    } 
} 
   
     