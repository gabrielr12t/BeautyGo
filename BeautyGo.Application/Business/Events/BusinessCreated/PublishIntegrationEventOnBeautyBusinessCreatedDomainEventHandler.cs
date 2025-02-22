using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Business;

namespace BeautyGo.Application.Business.Events.BusinessCreated;

internal class PublishIntegrationEventOnBeautyBusinessCreatedDomainEventHandler : IDomainEventHandler<EntityInsertedEvent<BeautyBusiness>>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PublishIntegrationEventOnBeautyBusinessCreatedDomainEventHandler(IIntegrationEventPublisher integrationEventPublisher)
        => _integrationEventPublisher = integrationEventPublisher;

    public async Task Handle(EntityInsertedEvent<BeautyBusiness> notification, CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.PublishAsync(new BeautyBusinessCreatedIntegrationEvent(notification.Entity));
    }
}
