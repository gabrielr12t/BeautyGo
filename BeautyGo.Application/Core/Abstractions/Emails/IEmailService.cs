using BeautyGo.Contracts.Emails;

namespace BeautyGo.Application.Core.Abstractions.Emails;

public interface IEmailService
{
    Task SendEmailAsync(MailRequest mailRequest, CancellationToken cancellationToken = default);
}
