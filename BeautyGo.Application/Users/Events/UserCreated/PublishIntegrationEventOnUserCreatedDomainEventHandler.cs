using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Application.Users.Events.UserCreated;

internal class PublishIntegrationEventOnUserCreatedDomainEventHandler : IDomainEventHandler<EntityInsertedEvent<User>>
{
    private readonly IEventBus _integrationEventPublisher;

    public PublishIntegrationEventOnUserCreatedDomainEventHandler(IEventBus integrationEventPublisher)
        => _integrationEventPublisher = integrationEventPublisher;

    public async Task Handle(EntityInsertedEvent<User> notification, CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.PublishAsync(new UserCreatedIntegrationEvent(notification.Entity));
    }
}