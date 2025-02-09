using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Stores;

namespace BeautyGo.Application.Stores.Events.StoreCreated;

internal class PublishIntegrationEventOnStoreCreatedDomainEventHandler : IDomainEventHandler<EntityInsertedEvent<Store>>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PublishIntegrationEventOnStoreCreatedDomainEventHandler(IIntegrationEventPublisher integrationEventPublisher)
        => _integrationEventPublisher = integrationEventPublisher;

    public async Task Handle(EntityInsertedEvent<Store> notification, CancellationToken cancellationToken)
    {
        _integrationEventPublisher.Publish(new StoreCreatedIntegrationEvent(notification.Entity));

        await Task.CompletedTask;
    }
}
