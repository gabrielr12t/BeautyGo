using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Entities.Notifications;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Infrastructure.Notifications;

internal class ProfessionalEmailNotificationPublisher : IProfessionalEmailNotificationPublisher
{
    private readonly IBaseRepository<EmailNotification> _emailRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProfessionalEmailNotificationPublisher(
        IBaseRepository<EmailNotification> emailRepository,
        IUnitOfWork unitOfWork)
    {
        _emailRepository = emailRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task PublishAsync(BusinessProfessionalAddedEmail message, CancellationToken cancellationToken = default)
    {
        var body = $"Você foi aceito como profissional do(a)  {message.Business}," +
            Environment.NewLine +
            Environment.NewLine +
            $"Agora você pode realizar sua agenda e atender seus clientes.";

        var notification = EmailNotification.Create(
            message.EmailTo,
            $"Novo Profissional do(a) {message.Business}! 🎉",
            body,
            DateTime.Now);

        await _emailRepository.InsertAsync(notification, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
