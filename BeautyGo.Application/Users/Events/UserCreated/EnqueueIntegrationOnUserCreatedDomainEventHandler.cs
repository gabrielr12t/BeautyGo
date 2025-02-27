﻿using BeautyGo.Application.Core.Abstractions.OutboxMessages;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Application.Users.Events.UserCreated;

internal class EnqueueIntegrationOnUserCreatedDomainEventHandler : IDomainEventHandler<EntityInsertedEvent<User>>
{
    private readonly IOutboxMessageService _outboxMessageService;

    public EnqueueIntegrationOnUserCreatedDomainEventHandler(IOutboxMessageService outboxMessageService) =>
        _outboxMessageService = outboxMessageService;

    public async Task Handle(EntityInsertedEvent<User> notification, CancellationToken cancellationToken) =>
        await _outboxMessageService.PublishAsync(new UserCreatedIntegrationEvent(notification.Entity), cancellationToken);
}