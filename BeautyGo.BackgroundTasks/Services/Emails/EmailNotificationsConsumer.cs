using BeautyGo.Application.Common.BackgroundServices;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Contracts.Emails;
using BeautyGo.Domain.Entities.Notifications;
using BeautyGo.Domain.Patterns.Specifications.Notifications;
using BeautyGo.Domain.Repositories;
using MediatR;

namespace BeautyGo.BackgroundTasks.Services.Emails;

internal sealed class EmailNotificationsConsumer : IEmailNotificationsConsumer
{
    private readonly IBaseRepository<EmailNotification> _emailNotificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailNotificationService _emailNotificationService;
    private readonly IMediator _mediator;

    public EmailNotificationsConsumer(
        IBaseRepository<EmailNotification> emailNotificationRepository,
        IUnitOfWork unitOfWork,
        IEmailNotificationService emailNotificationService,
        IMediator mediator)
    {
        _emailNotificationRepository = emailNotificationRepository;
        _unitOfWork = unitOfWork;
        _emailNotificationService = emailNotificationService;
        _mediator = mediator;
    }

    public async Task ConsumeAsync(int batchSize, CancellationToken cancellationToken = default)
    {
        var pendingEmailNotificationsSpecification = new PendingEmailNotificationsSpecification(DateTime.Now).Size(batchSize);
        var pendingEmailNotifications = await _emailNotificationRepository.GetAsync(pendingEmailNotificationsSpecification, false, cancellationToken);

        var sendNotificationEmailTasks = new List<Task>();

        foreach (EmailNotification notification in pendingEmailNotifications)
        {
            try
            {
                notification.IsSent = true;
                notification.SentDate = DateTime.Now;

                var notificationEmail = new NotificationEmail(
                    notification.RecipientEmail,
                    notification.Subject,
                    notification.Body);

                sendNotificationEmailTasks.Add(_emailNotificationService.SendAsync(notificationEmail, cancellationToken));

                _emailNotificationRepository.Update(notification);
            }
            catch (Exception ex)
            {
                notification.RetryCount++;
                notification.ErrorMessage = ex.Message;

                if (notification.RetryCount >= 3)
                {
                    notification.FailedDate = DateTime.UtcNow;

                    await _mediator.Publish(new EmailNotificationFailedEvent(notification.Id, ex), cancellationToken);
                }
            }
        }

        await Task.WhenAll(sendNotificationEmailTasks);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
