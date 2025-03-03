using BeautyGo.Contracts.Emails;

namespace BeautyGo.Application.Core.Abstractions.Notifications;

public interface IUserEmailNotificationPublisher  
{
    Task PublishAsync(PasswordChangedEmail message, CancellationToken cancellationToken = default);

    Task PublishAsync(WelcomeEmail message, CancellationToken cancellationToken = default);

    Task PublishAsync(ConfirmAccountEmail message, CancellationToken cancellationToken = default);
}
