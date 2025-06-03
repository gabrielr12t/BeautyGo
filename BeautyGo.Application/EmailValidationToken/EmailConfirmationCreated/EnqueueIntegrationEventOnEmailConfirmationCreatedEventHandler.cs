using BeautyGo.Application.Core.Abstractions.OutboxMessages;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.DomainEvents.EmailValidationToken;

namespace BeautyGo.Application.EmailValidationToken.EntityEmailValidationTokenCreated;

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
