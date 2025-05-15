using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Entities.Notifications;
using BeautyGo.Domain.Repositories.Bases;

namespace BeautyGo.Infrastructure.Notifications;

internal class ProfessionalRequestEmailNotificationPublisher : IProfessionalRequestEmailNotificationPublisher
{
    private readonly IEFBaseRepository<EmailNotification> _emailRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProfessionalRequestEmailNotificationPublisher(
        IEFBaseRepository<EmailNotification> emailRepository, 
        IUnitOfWork unitOfWork)
    {
        _emailRepository = emailRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task PublishAsync(ProfessionalRequestReminderEmail message, CancellationToken cancellationToken = default)
    {
        var body = $"Lembrete de convite para a loja {message.Business}," +
            Environment.NewLine +
            Environment.NewLine +
            $"Olá {message.Name}, seu convite para trabalhar na '{message.Business}' expira {message.ExpireIn:f}.";

        var notification = EmailNotification.Create(
            message.EmailTo,
            "Lembrete de convite!",
            body,
            DateTime.Now);

        await _emailRepository.InsertAsync(notification, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
