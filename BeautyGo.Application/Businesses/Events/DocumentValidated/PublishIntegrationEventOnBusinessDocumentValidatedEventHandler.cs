using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.DomainEvents.Businesses;

namespace BeautyGo.Application.Businesses.Events.DocumentValidated;

internal class PublishIntegrationEventOnBusinessDocumentValidatedEventHandler : IDomainEventHandler<BusinessDocumentValidatedDomainEvent>
{
    private readonly IEventBus _integrationEventPublisher;

    public PublishIntegrationEventOnBusinessDocumentValidatedEventHandler(IEventBus integrationEventPublisher)
    {
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(BusinessDocumentValidatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.PublishAsync(new BusinessDocumentValidatedIntegrationEvent(notification.Entity.Id));
    }
}
