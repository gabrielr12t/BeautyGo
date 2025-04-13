using BeautyGo.Application.ProfessionalInvitations.ProfessionalInvitationRequestSent;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.ProfessionalRequests.ProfessionalRequestSent;

internal class NotifyUserOnProfessionalRequestSentIntegrationEventHandler 
    : IIntegrationEventHandler<ProfessionalRequestSentIntegrationEvent>
{
    public Task Handle(ProfessionalRequestSentIntegrationEvent notification, CancellationToken cancellationToken)
    {
        //AVISAR POR EMAIL QUE A PESSOA TEM UM CONVITE PARA INTEGRAR A UMA 'LOJA'

        return Task.CompletedTask;
    }
}
