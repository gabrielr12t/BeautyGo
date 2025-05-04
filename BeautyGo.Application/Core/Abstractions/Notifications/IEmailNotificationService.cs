using BeautyGo.Contracts.Emails;

namespace BeautyGo.Application.Core.Abstractions.Notifications;

public interface IEmailNotificationService
{
    Task SendAsync(NotificationEmail notificationEmail, CancellationToken cancellationToken = default);
}
