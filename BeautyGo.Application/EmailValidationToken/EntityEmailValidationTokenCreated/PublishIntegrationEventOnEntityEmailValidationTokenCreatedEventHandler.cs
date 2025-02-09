using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities;

namespace BeautyGo.Application.EmailValidationToken.EntityEmailValidationTokenCreated;

internal class PublishIntegrationEventOnEntityEmailValidationTokenCreatedEventHandler
    : IEventHandler<EntityInsertedEvent<BeautyGoEmailTokenValidation>>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public PublishIntegrationEventOnEntityEmailValidationTokenCreatedEventHandler(IIntegrationEventPublisher integrationEventPublisher) => 
        _integrationEventPublisher = integrationEventPublisher;

    public async Task Handle(EntityInsertedEvent<BeautyGoEmailTokenValidation> notification, CancellationToken cancellationToken)
    {
        _integrationEventPublisher.Publish(new EntityEmailValidationTokenCreatedIntegrationEvent(notification.Entity));

        await Task.CompletedTask;
    }
}
