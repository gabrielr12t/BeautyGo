using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.DomainEvents.EmailValidationToken;

namespace BeautyGo.Application.EmailValidationToken.EntityEmailValidationTokenCreated;

internal class PublishIntegrationEventOnEntityEmailValidationTokenCreatedEventHandler
    : IDomainEventHandler<EmailValidationTokenCreatedEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PublishIntegrationEventOnEntityEmailValidationTokenCreatedEventHandler(IIntegrationEventPublisher integrationEventPublisher) => 
        _integrationEventPublisher = integrationEventPublisher;

    public async Task Handle(EmailValidationTokenCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.PublishAsync(new EmailValidationTokenCreatedIntegrationEvent(notification.Entity));
    }
}
