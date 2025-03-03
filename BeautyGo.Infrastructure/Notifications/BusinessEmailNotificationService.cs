using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Entities.Notifications;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Infrastructure.Notifications;

internal class BusinessEmailNotificationService : IBusinessEmailNotificationPublisher
{
    private readonly IBaseRepository<EmailNotification> _emailRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BusinessEmailNotificationService(
        IBaseRepository<EmailNotification> emailRepository,
        IUnitOfWork unitOfWork)
    {
        _emailRepository = emailRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task PublishAsync(ConfirmAccountEmail message, CancellationToken cancellationToken = default)
    {
        var body = $"Confirmação de email {message.Name}," +
            Environment.NewLine +
            Environment.NewLine +
            $"Clique no link para confirmar o cadastro da sua loja {message.Link}.";

        var mailRequest = EmailNotification.Create(
            message.EmailTo,
            "Confirmação de email! 🎉",
            body,
            DateTime.Now);

        await _emailRepository.InsertAsync(mailRequest, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task PublishAsync(DocumentValidatedEmail message, CancellationToken cancellationToken = default)
    {
        var body = $"Documento validado com sucesso," +
            Environment.NewLine +
            Environment.NewLine +
            "Agora você já pode agendar e trabalhar" +
            Environment.NewLine +
            Environment.NewLine +
            $"Clique no link para acessar a sua loja {message.Link}.";

        var mailRequest = EmailNotification.Create(
            message.EmailTo,
            "Documento validado com sucesso! 🎉",
            body,
            DateTime.Now);

        await _emailRepository.InsertAsync(mailRequest, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
