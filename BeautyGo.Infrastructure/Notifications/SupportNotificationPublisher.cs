using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Entities.Notifications;
using BeautyGo.Domain.Repositories;
using BeautyGo.Domain.Settings;

namespace BeautyGo.Infrastructure.Notifications;

public class SupportNotificationPublisher : ISupportNotificationPublisher
{
    private readonly IBaseRepository<EmailNotification> _emailRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly MailSettings _mailSettings;

    public SupportNotificationPublisher(
        IBaseRepository<EmailNotification> emailRepository,
        IUnitOfWork unitOfWork,
        AppSettings appSettings)
    {
        _emailRepository = emailRepository;
        _unitOfWork = unitOfWork;
        _mailSettings = appSettings.Get<MailSettings>();
    }

    public async Task PublishAsync(SupportBackgroundFailedEmail message, CancellationToken cancellationToken = default)
    {
        var notification = EmailNotification.Create(
            _mailSettings.SupportEmail,
            message.Subject,
            message.Body,
            DateTime.Now);

        await _emailRepository.InsertAsync(notification, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
