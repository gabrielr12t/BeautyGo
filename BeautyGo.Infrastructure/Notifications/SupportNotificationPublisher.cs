using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Entities.Notifications;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Infrastructure.Notifications;

public class SupportNotificationPublisher : ISupportNotificationPublisher
{
    private readonly IBaseRepository<EmailNotification> _emailRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SupportNotificationPublisher(
        IBaseRepository<EmailNotification> emailRepository, 
        IUnitOfWork unitOfWork)
    {
        _emailRepository = emailRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task PublishAsync(SupportBackgroundFailedEmail message, CancellationToken cancellationToken = default)
    {
        var notification = EmailNotification.Create(
            message.EmailTo,
            message.Subject,
            message.Body,
            DateTime.Now);

        await _emailRepository.InsertAsync(notification, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
