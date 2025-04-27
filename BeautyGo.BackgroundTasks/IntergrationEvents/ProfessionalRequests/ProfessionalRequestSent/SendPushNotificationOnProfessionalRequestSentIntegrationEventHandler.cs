using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Application.ProfessionalRequests.ProfessionalRequestSent;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.ProfessionalRequests.ProfessionalRequestSent;

internal class SendPushNotificationOnProfessionalRequestSentIntegrationEventHandler
    : IIntegrationEventHandler<ProfessionalRequestSentIntegrationEvent>
{
    private readonly IPushNotificationService _pushtNotificationService;

    public SendPushNotificationOnProfessionalRequestSentIntegrationEventHandler(
        IPushNotificationService pushtNotificationService)
    {
        _pushtNotificationService = pushtNotificationService;
    }

    public async Task Handle(ProfessionalRequestSentIntegrationEvent notification, CancellationToken cancellationToken)
    {
        //enviar push para o usuário destinatário

        await _pushtNotificationService.SendAsync("Convite para ser funcionário", string.Empty, string.Empty);
    }
}
