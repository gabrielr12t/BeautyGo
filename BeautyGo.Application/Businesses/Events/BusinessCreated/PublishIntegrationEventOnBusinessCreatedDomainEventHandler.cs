using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Businesses;

namespace BeautyGo.Application.Businesses.Events.BusinessCreated;

internal class PublishIntegrationEventOnBusinessCreatedDomainEventHandler : IDomainEventHandler<EntityInsertedEvent<Business>>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PublishIntegrationEventOnBusinessCreatedDomainEventHandler(IIntegrationEventPublisher integrationEventPublisher)
        => _integrationEventPublisher = integrationEventPublisher;

    public async Task Handle(EntityInsertedEvent<Business> notification, CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.PublishAsync(new BusinessCreatedIntegrationEvent(notification.Entity));
    }
}
