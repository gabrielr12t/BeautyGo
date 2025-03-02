using BeautyGo.Application.Core.Abstractions.Emails;
using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Contracts.Emails;

namespace BeautyGo.Infrastructure.Notifications;

internal class EmailNotificationService : IEmailNotificationService
{
    private readonly IEmailService _emailService;

    public EmailNotificationService(
        IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task SendAsync(NotificationEmail notificationEmail)
    {
        var mailRequest = new MailRequest(notificationEmail.EmailTo, notificationEmail.Subject, notificationEmail.Body);

        await _emailService.SendEmailAsync(mailRequest);
    }

    public async Task SendAsync(PasswordChangedEmail message)
    {
        var mailRequest = new MailRequest(
            message.EmailTo,
            "Senha alterada 🔐",
            $"Olá {message.Name}," +
            Environment.NewLine +
            Environment.NewLine +
            "Sua senha foi alterada com sucesso.");

        await _emailService.SendEmailAsync(mailRequest);
    }

    public async Task SendAsync(WelcomeEmail message)
    {
        var mailRequest = new MailRequest(
            message.EmailTo,
            "Bem vindo ao BeautyGo! 🎉",
            $"Bem vindo ao BeautyGo  {message.Name}," +
            Environment.NewLine +
            Environment.NewLine +
            $"Registrado com o e-mail {message.EmailTo}.");

        await _emailService.SendEmailAsync(mailRequest);
    }

    public async Task SendAsync(UserConfirmEmail message)
    {
        var mailRequest = new MailRequest(
            message.EmailTo,
            "Confirmação de email! 🎉",
            $"Confirmação de email {message.Name}," +
            Environment.NewLine +
            Environment.NewLine +
            $"Clique no link para confirmar seu cadastro {message.Link}.");

        await _emailService.SendEmailAsync(mailRequest);
    }

    public async Task SendAsync(BeautyBusinessConfirmEmail message)
    {
        var mailRequest = new MailRequest(
            message.EmailTo,
            "Confirmação de email! 🎉",
            $"Confirmação de email {message.Name}," +
            Environment.NewLine +
            Environment.NewLine +
            $"Clique no link para confirmar o cadastro da sua loja {message.Link}.");

        await _emailService.SendEmailAsync(mailRequest);
    }
}
