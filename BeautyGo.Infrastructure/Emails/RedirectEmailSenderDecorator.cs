using BeautyGo.Application.Core.Abstractions.Emails;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Settings;

namespace BeautyGo.Infrastructure.Emails;

internal class RedirectEmailSenderDecorator : IEmailService
{
    private readonly IEmailService _innerEmailService;
    private readonly RedirectMailSettings _mailSettings;

    public RedirectEmailSenderDecorator(
        IEmailService innerEmailService,
        RedirectMailSettings mailSettings)
    {
        _innerEmailService = innerEmailService;
        _mailSettings = mailSettings;
    }

    public async Task SendEmailAsync(MailRequest mailRequest, CancellationToken cancellationToken = default)
    {
        if (_mailSettings.RedirectEmailsInHomologation)
        {
            mailRequest.EmailTo = _mailSettings.RedirectEmailTo;
        }

        await _innerEmailService.SendEmailAsync(mailRequest, cancellationToken);
    }
}
