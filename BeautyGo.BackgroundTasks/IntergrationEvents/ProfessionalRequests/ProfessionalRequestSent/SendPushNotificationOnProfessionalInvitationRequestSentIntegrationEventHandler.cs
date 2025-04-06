using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Application.ProfessionalInvitations.ProfessionalInvitationRequestSent;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.ProfessionalRequests.ProfessionalRequestSent;

internal class SendPushNotificationOnProfessionalInvitationRequestSentIntegrationEventHandler
    : IIntegrationEventHandler<ProfessionalInvitationRequestSentIntegrationEvent>
{
    private readonly IPushNotificationService _pushtNotificationService;

    public SendPushNotificationOnProfessionalInvitationRequestSentIntegrationEventHandler(
        IPushNotificationService pushtNotificationService)
    {
        _pushtNotificationService = pushtNotificationService;
    }

    public async Task Handle(ProfessionalInvitationRequestSentIntegrationEvent notification, CancellationToken cancellationToken)
    {
        //enviar push para o usuário destinatário

        await _pushtNotificationService.SendAsync("Convite para ser funcionário", string.Empty, string.Empty);
    }
}
