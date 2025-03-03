using BeautyGo.Contracts.Emails;

namespace BeautyGo.Application.Core.Abstractions.Notifications;

public interface IBusinessEmailNotificationPublisher 
{
    Task PublishAsync(ConfirmAccountEmail message, CancellationToken cancellationToken = default);

    Task PublishAsync(DocumentValidatedEmail message, CancellationToken cancellationToken = default);
}
