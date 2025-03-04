namespace BeautyGo.BackgroundTasks.Services.Events;

public interface IEventProcessor
{
    Task ProduceAsync(int batchSize, CancellationToken cancellationToken);
}
