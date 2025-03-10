namespace BeautyGo.Application.Core.Abstractions.Messaging;

public interface IPublisherBusEvent
{
    Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken = default);  
}
