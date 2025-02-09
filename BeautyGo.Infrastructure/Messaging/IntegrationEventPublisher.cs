using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Settings;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace BeautyGo.Infrastructure.Messaging;

internal sealed class IntegrationEventPublisher : IIntegrationEventPublisher, IDisposable
{
    private readonly MessageBrokerSettings _messageBrokerSettings;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public IntegrationEventPublisher(AppSettings appSettings)
    {
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
        initializer.ConfigureQueue(_messageBrokerSettings.QueueName, "dlx_exchange", 60000);
    }

    public void Publish(IIntegrationEvent @event)
    {
        var payload = JsonConvert.SerializeObject(@event, typeof(IIntegrationEvent), new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
        });

        var body = Encoding.UTF8.GetBytes(payload);

        _channel.BasicPublish(string.Empty, _messageBrokerSettings.QueueName, body: body);
    }

    public void Dispose()
    {
        _connection.Dispose();
        _channel?.Dispose();
    }
}