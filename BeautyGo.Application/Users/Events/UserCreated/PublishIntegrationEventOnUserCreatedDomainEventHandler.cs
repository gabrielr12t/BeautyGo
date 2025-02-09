using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Application.Users.Events.UserCreated;

internal class PublishIntegrationEventOnUserCreatedDomainEventHandler : IDomainEventHandler<EntityInsertedEvent<User>>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PublishIntegrationEventOnUserCreatedDomainEventHandler(IIntegrationEventPublisher integrationEventPublisher)
        => _integrationEventPublisher = integrationEventPublisher;

    public async Task Handle(EntityInsertedEvent<User> notification, CancellationToken cancellationToken)
    {
        _integrationEventPublisher.Publish(new UserCreatedIntegrationEvent(notification.Entity));

        await Task.CompletedTask;
    }
}