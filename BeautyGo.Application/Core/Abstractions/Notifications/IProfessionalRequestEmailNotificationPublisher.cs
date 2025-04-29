using BeautyGo.Contracts.Emails;

namespace BeautyGo.Application.Core.Abstractions.Notifications;

public interface IProfessionalRequestEmailNotificationPublisher
{
    Task PublishAsync(ProfessionalRequestReminderEmail message, CancellationToken cancellationToken = default);
}
