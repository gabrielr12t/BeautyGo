using BeautyGo.Contracts.Emails;

namespace BeautyGo.Application.Core.Abstractions.Notifications;

public interface IProfessionalEmailNotificationPublisher
{
    Task PublishAsync(BusinessProfessionalAddedEmail message, CancellationToken cancellationToken = default);
    Task PublishAsync(ProfessionalRequestInviteEmail message, CancellationToken cancellationToken = default);
}
