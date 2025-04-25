using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Entities.Notifications;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Infrastructure.Notifications;

internal class UserEmailNotificationPublisher : IUserEmailNotificationPublisher
{
    private readonly IBaseRepository<EmailNotification> _emailRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserEmailNotificationPublisher(
        IBaseRepository<EmailNotification> emailRepository,
        IUnitOfWork unitOfWork)
    {
        _emailRepository = emailRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task PublishAsync(PasswordChangedEmail message, CancellationToken cancellationToken = default)
    {
        var body = $"Olá {message.Name}," +
            Environment.NewLine +
            Environment.NewLine +
            "Sua senha foi alterada com sucesso.";

        var notification = EmailNotification.Create(
            message.EmailTo,
            "Senha alterada 🔐",
            body, DateTime.Now);

        await _emailRepository.InsertAsync(notification, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task PublishAsync(WelcomeEmail message, CancellationToken cancellationToken = default)
    {
        var body = $"Bem vindo ao BeautyGo  {message.Name}," +
            Environment.NewLine +
            Environment.NewLine +
            $"Registrado com o e-mail {message.EmailTo}.";

        var notification = EmailNotification.Create(
            message.EmailTo,
            "Bem vindo ao BeautyGo! 🎉",
            body,
            DateTime.Now);

        await _emailRepository.InsertAsync(notification, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task PublishAsync(ConfirmAccountEmail message, CancellationToken cancellationToken = default)
    {
        var body = $"Confirmação de email {message.Name}," +
            Environment.NewLine +
            Environment.NewLine +
            $"Clique no link para confirmar seu cadastro {message.Link}.";

        var notification = EmailNotification.Create(
            message.EmailTo,
            "Confirmação de email! 🎉",
            body,
            DateTime.Now);

        await _emailRepository.InsertAsync(notification, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
