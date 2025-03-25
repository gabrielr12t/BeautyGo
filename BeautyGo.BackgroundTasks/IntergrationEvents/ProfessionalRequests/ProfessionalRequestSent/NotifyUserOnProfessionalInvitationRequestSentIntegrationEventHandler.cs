using BeautyGo.Application.ProfessionalInvitations.ProfessionalInvitationRequestSent;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.ProfessionalRequests.ProfessionalRequestSent;

internal class NotifyUserOnProfessionalInvitationRequestSentIntegrationEventHandler 
    : IIntegrationEventHandler<ProfessionalInvitationRequestSentIntegrationEvent>
{
    public Task Handle(ProfessionalInvitationRequestSentIntegrationEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
