namespace BeautyGo.Application.Core.Abstractions.Messaging;

public interface IEventBus
{
    Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken = default);  
}
