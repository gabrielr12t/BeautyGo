using BeautyGo.Application.Core.Abstractions.Emails;
using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Contracts.Emails;

namespace BeautyGo.Infrastructure.Notifications;

internal class EmailNotificationService : IEmailNotificationService
{
    public readonly IEmailService _emailService;

    public EmailNotificationService(
        IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task SendAsync(NotificationEmail notificationEmail, CancellationToken cancellationToken = default)
    {
        var mailRequest = new MailRequest(notificationEmail.EmailTo, notificationEmail.Subject, notificationEmail.Body);

        await _emailService.SendEmailAsync(mailRequest, cancellationToken);
    }
}
