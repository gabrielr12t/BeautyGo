using BeautyGo.Application.Core.Abstractions.OutboxMessages;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.DomainEvents.Businesses;

namespace BeautyGo.Application.Businesses.Commands.AccountConfirmed;

internal class EnqueueIntegrationEventOnbusinessAccountConfirmedEventHandler : IDomainEventHandler<BusinessAccountConfirmedDomainEvent>
{
    private readonly IOutboxMessageService _outboxMessageService;

    public EnqueueIntegrationEventOnbusinessAccountConfirmedEventHandler(
        IOutboxMessageService outboxMessageService)
    {
        _outboxMessageService = outboxMessageService;
    }

    public async Task Handle(BusinessAccountConfirmedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _outboxMessageService.PublishAsync(
            new BusinessAccountConfirmedIntegrationEvent(notification.Business),
            cancellationToken);
    }
}
