using BeautyGo.Contracts.Emails;

namespace BeautyGo.Application.Core.Abstractions.Notifications;

public interface IBusinessEmailNotificationService : IEmailNotificationService
{
    Task SendAsync(BusinessConfirmEmail message);
}
