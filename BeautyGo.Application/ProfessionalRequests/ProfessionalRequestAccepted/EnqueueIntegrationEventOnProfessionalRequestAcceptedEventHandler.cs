using BeautyGo.Application.Core.Abstractions.OutboxMessages;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.DomainEvents;

namespace BeautyGo.Application.ProfessionalRequests.ProfessionalRequestAccepted;

internal class EnqueueIntegrationEventOnProfessionalRequestAcceptedEventHandler : IDomainEventHandler<ProfessionalRequestAcceptedDomainEvent>
{
    private readonly IOutboxMessageService _outboxMessageService;

    public EnqueueIntegrationEventOnProfessionalRequestAcceptedEventHandler(IOutboxMessageService outboxMessageService)
    {
        _outboxMessageService = outboxMessageService;
    }

    public async Task Handle(ProfessionalRequestAcceptedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _outboxMessageService.PublishAsync(
            new ProfessionalRequestAcceptedIntegrationEvent(notification.ProfessionalRequest.Id), cancellationToken);
    }
}
