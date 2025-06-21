using BeautyGo.Application.Core.Abstractions.OutboxMessages;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.DomainEvents;

namespace BeautyGo.Application.Users.Commands.UserIpAddressChanged;

internal class EnqueueIntegrationOnUserIpAddressChangedDomainEventHandler : IDomainEventHandler<UserIpAddressChangedDomainEvent>
{
    private readonly IOutboxMessageService _outboxMessageService;

    public EnqueueIntegrationOnUserIpAddressChangedDomainEventHandler(IOutboxMessageService outboxMessageService) =>
        _outboxMessageService = outboxMessageService;

    public async Task Handle(UserIpAddressChangedDomainEvent notification, CancellationToken cancellationToken) =>
        await _outboxMessageService.PublishAsync(new UserIpAddressChangedIntegrationEvent(notification.User), cancellationToken);
}