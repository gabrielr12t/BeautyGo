namespace BeautyGo.BackgroundTasks.Services.OutboxMessages;

public interface IProcessOutboxMessagesProducer
{
    Task ProduceAsync(CancellationToken cancellationToken);
}
