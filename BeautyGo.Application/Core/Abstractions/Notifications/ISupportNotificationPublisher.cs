using BeautyGo.Contracts.Emails;

namespace BeautyGo.Application.Core.Abstractions.Notifications;

public interface ISupportNotificationPublisher
{
    Task PublishAsync(SupportBackgroundFailedEmail message, CancellationToken cancellationToken = default);
}
