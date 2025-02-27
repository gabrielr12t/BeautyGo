using BeautyGo.Application.Core.Abstractions.OutboxMessages;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Businesses;

namespace BeautyGo.Application.Businesses.Events.BusinessCreated;

internal class EnqueueIntegrationEventOnBusinessCreatedDomainEventHandler : IDomainEventHandler<EntityInsertedEvent<Business>>
{
    private readonly IOutboxMessageService _outboxMessageService;

    public EnqueueIntegrationEventOnBusinessCreatedDomainEventHandler(IOutboxMessageService outboxMessageService)
    {
        _outboxMessageService = outboxMessageService;
    }

    public async Task Handle(EntityInsertedEvent<Business> notification, CancellationToken cancellationToken)
    {
        await _outboxMessageService.PublishAsync(new BusinessCreatedIntegrationEvent(notification.Entity), cancellationToken);
    }
}
