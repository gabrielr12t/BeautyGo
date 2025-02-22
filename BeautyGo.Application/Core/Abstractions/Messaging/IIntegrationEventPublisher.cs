namespace BeautyGo.Application.Core.Abstractions.Messaging;

public interface IIntegrationEventPublisher
{
    Task PublishAsync(IIntegrationEvent @event);  
}
