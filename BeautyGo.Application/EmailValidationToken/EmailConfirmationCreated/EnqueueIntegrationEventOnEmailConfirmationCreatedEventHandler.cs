using BeautyGo.Application.Core.Abstractions.OutboxMessages;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.DomainEvents;
using BeautyGo.Domain.Entities;

namespace BeautyGo.Application.EmailValidationToken.EmailConfirmationCreated;

internal class EnqueueIntegrationEventOnEmailConfirmationCreatedEventHandler : IDomainEventHandler<EntityInsertedDomainEvent<EmailConfirmation>>
{
    private readonly IOutboxMessageService _outboxMessageService;

    public EnqueueIntegrationEventOnEmailConfirmationCreatedEventHandler(IOutboxMessageService outboxMessageService)
    {
        _outboxMessageService = outboxMessageService;
    }

    public async Task Handle(EntityInsertedDomainEvent<EmailConfirmation> notification, CancellationToken cancellationToken)
    {
        await _outboxMessageService.PublishAsync(new EmailConfirmationCreatedIntegrationEvent(notification.Entity), cancellationToken);
    }
}
