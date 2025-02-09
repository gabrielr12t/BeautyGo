using BeautyGo.Contracts.Emails;

namespace BeautyGo.Application.Core.Abstractions.Notifications;

public interface IEmailNotificationService
{
    Task SendAsync(WelcomeEmail welcomeEmail);

    Task SendAsync(UserConfirmEmail message);

    Task SendAsync(StoreConfirmEmail message);

    Task SendAsync(PasswordChangedEmail passwordChangedEmail);
     
    Task SendAsync(NotificationEmail notificationEmail);
}
