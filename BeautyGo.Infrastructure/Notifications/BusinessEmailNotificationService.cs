using BeautyGo.Application.Core.Abstractions.Emails;
using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Contracts.Emails;

namespace BeautyGo.Infrastructure.Notifications;

internal class BusinessEmailNotificationService : EmailNotificationService, IBusinessEmailNotificationService
{
    public BusinessEmailNotificationService(IEmailService emailService) 
        : base(emailService)
    {
    }

    public async Task SendAsync(BusinessConfirmEmail message)
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
