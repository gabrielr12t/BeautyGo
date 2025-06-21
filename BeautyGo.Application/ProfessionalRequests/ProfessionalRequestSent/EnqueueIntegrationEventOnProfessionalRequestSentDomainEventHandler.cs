using BeautyGo.Application.Core.Abstractions.OutboxMessages;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.DomainEvents;

namespace BeautyGo.Application.ProfessionalRequests.ProfessionalRequestSent;

public class EnqueueIntegrationEventOnProfessionalRequestSentDomainEventHandler : IDomainEventHandler<ProfessionalRequestSentDomainEvent>
{
    private readonly IOutboxMessageService _outboxMessageService;

    public EnqueueIntegrationEventOnProfessionalRequestSentDomainEventHandler(IOutboxMessageService outboxMessageService)
    {
        _outboxMessageService = outboxMessageService;
    }

    public async Task Handle(ProfessionalRequestSentDomainEvent notification, CancellationToken cancellationToken)
    {
        await _outboxMessageService.PublishAsync(new ProfessionalRequestSentIntegrationEvent(notification.ProfessionalRequest.Id), cancellationToken);
    }
}
