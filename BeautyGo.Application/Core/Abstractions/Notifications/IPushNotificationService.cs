namespace BeautyGo.Application.Core.Abstractions.Notifications;

public interface IPushNotificationService
{
    Task<bool> SendAsync(string title, string body, string deviceToken);
}
