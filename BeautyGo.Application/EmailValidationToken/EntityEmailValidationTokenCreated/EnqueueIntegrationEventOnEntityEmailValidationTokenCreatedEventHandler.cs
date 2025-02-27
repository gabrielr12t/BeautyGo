using BeautyGo.Application.Core.Abstractions.OutboxMessages;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.DomainEvents.EmailValidationToken;

namespace BeautyGo.Application.EmailValidationToken.EntityEmailValidationTokenCreated;

internal class EnqueueIntegrationEventOnEntityEmailValidationTokenCreatedEventHandler : IDomainEventHandler<EmailValidationTokenCreatedEvent>
{
    private readonly IOutboxMessageService _outboxMessageService;

    public EnqueueIntegrationEventOnEntityEmailValidationTokenCreatedEventHandler(IOutboxMessageService outboxMessageService)
    {
        _outboxMessageService = outboxMessageService;
    }

    public async Task Handle(EmailValidationTokenCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _outboxMessageService.PublishAsync(new EmailValidationTokenCreatedIntegrationEvent(notification.Entity), cancellationToken);
    }
}
