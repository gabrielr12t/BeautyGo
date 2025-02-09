using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace BeautyGo.Infrastructure.Messaging;

public sealed class RabbitMQInitializer
{
    private readonly IModel _channel;

    public RabbitMQInitializer(IModel channel)
    {
        _channel = channel;
    }

    public void ConfigureQueue(string queueName, string deadLetterExchange, int messageTtl)
    {
        try
        {
            _channel.QueueDeclarePassive(queueName);
        }
        catch (OperationInterruptedException)
        {
            var args = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", deadLetterExchange },
            { "x-message-ttl", messageTtl }
        };

            _channel.QueueDeclare(queueName, true, false, false, args);
        }
    }
}
