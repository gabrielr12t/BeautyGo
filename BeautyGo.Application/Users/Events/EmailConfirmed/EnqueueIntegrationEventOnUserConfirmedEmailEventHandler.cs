using BeautyGo.Application.Core.Abstractions.OutboxMessages;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.DomainEvents.Users;

namespace BeautyGo.Application.Users.Events.EmailConfirmed;

internal class EnqueueIntegrationEventOnUserConfirmedEmailEventHandler : IDomainEventHandler<UserConfirmEmailDomainEvent>
{
    private readonly IOutboxMessageService _outboxMessageService;

    public EnqueueIntegrationEventOnUserConfirmedEmailEventHandler(IOutboxMessageService outboxMessageService)
    {
        _outboxMessageService = outboxMessageService;
    }

    public async Task Handle(UserConfirmEmailDomainEvent notification, CancellationToken cancellationToken)
    {
        await _outboxMessageService.PublishAsync(new UserConfirmedEmailIntegrationEvent(notification.User), cancellationToken);
    }
}
