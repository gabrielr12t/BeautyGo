﻿using BeautyGo.Application.Core.Abstractions.OutboxMessages;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.DomainEvents;

namespace BeautyGo.Application.EmailValidationToken.EmailConfirmationCreated;

internal class EnqueueIntegrationEventOnEmailConfirmationCreatedEventHandler : IDomainEventHandler<EmailValidationTokenCreatedEvent>
{
    private readonly IOutboxMessageService _outboxMessageService;

    public EnqueueIntegrationEventOnEmailConfirmationCreatedEventHandler(IOutboxMessageService outboxMessageService)
    {
        _outboxMessageService = outboxMessageService;
    }

    public async Task Handle(EmailValidationTokenCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _outboxMessageService.PublishAsync(new EmailConfirmationCreatedIntegrationEvent(notification.Entity), cancellationToken);
    }
}
