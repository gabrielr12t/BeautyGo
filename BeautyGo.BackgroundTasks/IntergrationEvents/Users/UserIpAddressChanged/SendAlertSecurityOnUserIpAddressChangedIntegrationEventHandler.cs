using BeautyGo.Application.Users.Commands.UserIpAddressChanged;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.Users.UserIpAddressChanged;

internal class SendAlertSecurityOnUserIpAddressChangedIntegrationEventHandler : IIntegrationEventHandler<UserIpAddressChangedIntegrationEvent>
{
    public Task Handle(UserIpAddressChangedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        // IMPLEMENTAR
        // NOTIFICAR USUÁRIO AO FAZER ALGUMA ATIVIDADE DE UM IP DIFERENTE DO DELE

        return Task.CompletedTask;
    }
}
