using BeautyGo.Application.Core.Abstractions.OutboxMessages;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Businesses;

namespace BeautyGo.Application.Businesses.Commands.BusinessCreated;

public class EnqueueIntegrationEventOnBusinessCreatedDomainEventHandler : IDomainEventHandler<EntityInsertedDomainEvent<Business>>
{
    private readonly IOutboxMessageService _outboxMessageService;

    public EnqueueIntegrationEventOnBusinessCreatedDomainEventHandler(IOutboxMessageService outboxMessageService)
    {
        _outboxMessageService = outboxMessageService;
    }

    public async Task Handle(EntityInsertedDomainEvent<Business> notification, CancellationToken cancellationToken)
    {
        await _outboxMessageService.PublishAsync(
            new BusinessCreatedIntegrationEvent(notification.Entity), 
            cancellationToken);
    }
}
