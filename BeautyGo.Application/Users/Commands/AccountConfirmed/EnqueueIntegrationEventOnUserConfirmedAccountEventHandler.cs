using BeautyGo.Application.Core.Abstractions.OutboxMessages;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.DomainEvents;

namespace BeautyGo.Application.Users.Commands.AccountConfirmed;

internal class EnqueueIntegrationEventOnUserConfirmedAccountEventHandler : IDomainEventHandler<UserConfirmedAccountDomainEvent>
{
    private readonly IOutboxMessageService _outboxMessageService;

    public EnqueueIntegrationEventOnUserConfirmedAccountEventHandler(IOutboxMessageService outboxMessageService)
    {
        _outboxMessageService = outboxMessageService;
    }

    public async Task Handle(UserConfirmedAccountDomainEvent notification, CancellationToken cancellationToken)
    {
        await _outboxMessageService.PublishAsync(
            new UserConfirmedAccountIntegrationEvent(notification.User), 
            cancellationToken);
    }
}
