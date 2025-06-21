using BeautyGo.Application.Core.Abstractions.OutboxMessages;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Application.Users.Commands.UserCreated;

internal class EnqueueIntegrationOnUserCreatedDomainEventHandler : IDomainEventHandler<EntityInsertedDomainEvent<User>>
{
    private readonly IOutboxMessageService _outboxMessageService;

    public EnqueueIntegrationOnUserCreatedDomainEventHandler(IOutboxMessageService outboxMessageService) =>
        _outboxMessageService = outboxMessageService;

    public async Task Handle(EntityInsertedDomainEvent<User> notification, CancellationToken cancellationToken) =>
        await _outboxMessageService.PublishAsync(new UserCreatedIntegrationEvent(notification.Entity), cancellationToken);
}