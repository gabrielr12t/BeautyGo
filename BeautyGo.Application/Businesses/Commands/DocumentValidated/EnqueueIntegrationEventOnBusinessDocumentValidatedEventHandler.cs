﻿using BeautyGo.Application.Core.Abstractions.OutboxMessages;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.DomainEvents;

namespace BeautyGo.Application.Businesses.Commands.DocumentValidated;

internal class EnqueueIntegrationEventOnBusinessDocumentValidatedEventHandler : IDomainEventHandler<BusinessDocumentValidatedDomainEvent>
{
    private readonly IOutboxMessageService _outboxMessageService;

    public EnqueueIntegrationEventOnBusinessDocumentValidatedEventHandler(IOutboxMessageService outboxMessageService)
    {
        _outboxMessageService = outboxMessageService;
    }

    public async Task Handle(BusinessDocumentValidatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _outboxMessageService.PublishAsync(new BusinessDocumentValidatedIntegrationEvent(notification.Business.Id), cancellationToken);
    }
}
