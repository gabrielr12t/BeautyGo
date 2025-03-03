using BeautyGo.Contracts.Emails;

namespace BeautyGo.Application.Core.Abstractions.Notifications;

public interface IUserEmailNotificationService : IEmailNotificationService
{
    Task SendAsync(PasswordChangedEmail message);

    Task SendAsync(WelcomeEmail message);

    Task SendAsync(UserConfirmEmail message);
}
