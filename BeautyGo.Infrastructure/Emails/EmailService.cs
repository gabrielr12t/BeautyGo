using BeautyGo.Application.Core.Abstractions.Emails;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace BeautyGo.Infrastructure.Emails;

internal class EmailService : IEmailService
{
    private readonly MailSettings _mailSettings;

    public EmailService(AppSettings appSettings) =>
        _mailSettings = appSettings.Get<MailSettings>();

    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        var email = new MimeMessage
        {
            From = { new MailboxAddress(_mailSettings.SenderDisplayName, _mailSettings.SenderEmail) },
            To = { MailboxAddress.Parse(mailRequest.EmailTo) },
            Subject = mailRequest.Subject,
            Body = new TextPart(TextFormat.Text) { Text = mailRequest.Body }
        };

        using var smtpClient = new SmtpClient();

        await smtpClient.ConnectAsync(_mailSettings.SmtpServer, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);

        await smtpClient.AuthenticateAsync(_mailSettings.SenderEmail, _mailSettings.SmtpAppPassword);

        await smtpClient.SendAsync(email);

        await smtpClient.DisconnectAsync(true);
    }
}