namespace BeautyGo.Contracts.Emails;

public record NotificationEmail(
    string EmailTo,
    string Subject,
    string Body) : IEmailNotification; 