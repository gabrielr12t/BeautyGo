using BeautyGo.Application.Core.Abstractions.OutboxMessages;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.DomainEvents.Professionals;

namespace BeautyGo.Application.ProfessionalInvitations.ProfessionalInvitationRequestSent;

public class EnqueueIntegrationEventOnProfessionalRequestSentDomainEventHandler : IDomainEventHandler<ProfessionalRequestSentDomainEvent>
{
    private readonly IOutboxMessageService _outboxMessageService;

    public EnqueueIntegrationEventOnProfessionalRequestSentDomainEventHandler(IOutboxMessageService outboxMessageService)
    {
        _outboxMessageService = outboxMessageService;
    }

    public async Task Handle(ProfessionalRequestSentDomainEvent notification, CancellationToken cancellationToken)
    {
        await _outboxMessageService.PublishAsync(new ProfessionalInvitationRequestSentIntegrationEvent(notification.ProfessionalRequest.Id), cancellationToken);
    }
}
