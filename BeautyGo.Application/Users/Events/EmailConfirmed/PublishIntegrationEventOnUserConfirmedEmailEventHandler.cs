using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.DomainEvents.Users;

namespace BeautyGo.Application.Users.Events.EmailConfirmed;

internal class PublishIntegrationEventOnUserConfirmedEmailEventHandler : IDomainEventHandler<UserConfirmEmailDomainEvent>
{
    private readonly IEventBus _integrationEventPublisher;

    public PublishIntegrationEventOnUserConfirmedEmailEventHandler(IEventBus integrationEventPublisher) =>
        _integrationEventPublisher = integrationEventPublisher;

    public async Task Handle(UserConfirmEmailDomainEvent notification, CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.PublishAsync(new UserConfirmedEmailIntegrationEvent(notification.User));
    }
}
