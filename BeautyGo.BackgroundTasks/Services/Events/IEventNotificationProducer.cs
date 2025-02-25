namespace BeautyGo.BackgroundTasks.Services.Events;

public interface IEventNotificationProducer
{
    Task ProduceAsync(int batchSize, CancellationToken cancellationToken);
}
