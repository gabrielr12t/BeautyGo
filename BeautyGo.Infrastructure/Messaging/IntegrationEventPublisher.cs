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

internal sealed class IntegrationEventPublisher : IIntegrationEventPublisher, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly MessageBrokerSettings _messageBrokerSettings;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public IntegrationEventPublisher(AppSettings appSettings, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _messageBrokerSettings = appSettings.Get<MessageBrokerSettings>();

        IAsyncConnectionFactory connectionFactory = new ConnectionFactory
        {
            HostName = _messageBrokerSettings.HostName,
            Port = _messageBrokerSettings.Port,
            UserName = _messageBrokerSettings.UserName,
            Password = _messageBrokerSettings.Password,
            DispatchConsumersAsync = true
        };

        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

        var initializer = new RabbitMQInitializer(_channel);

        _channel.QueueDeclare(_messageBrokerSettings.QueueName, false, false, false);
    }
     
    public async Task PublishAsync(IIntegrationEvent @event)
    {
        using var scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var payload = JsonConvert.SerializeObject(@event, typeof(IIntegrationEvent), new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
        });

        var body = Encoding.UTF8.GetBytes(payload);

        _channel.BasicPublish(string.Empty, _messageBrokerSettings.QueueName, body: body);

        await logger.InformationAsync($"MESSAGE: {@event.GetType()} - Publishing - {@event}");

        await unitOfWork.SaveChangesAsync();
    }

    public void Dispose()
    {
        _connection.Dispose();
        _channel?.Dispose();
    }
}