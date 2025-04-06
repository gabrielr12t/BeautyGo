using BeautyGo.Application.Core.Abstractions.Notifications;

namespace BeautyGo.Infrastructure.Notifications;

internal class PushNotificationService : IPushNotificationService
{
    public Task<bool> SendAsync(string title, string body, string deviceToken)
    {
        // IMPLEMENTAR PUSH COM FIREBASE

        return Task.FromResult(true);
    }
}
