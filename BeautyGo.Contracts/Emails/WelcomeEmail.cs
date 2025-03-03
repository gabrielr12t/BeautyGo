namespace BeautyGo.Contracts.Emails;

public record WelcomeEmail(
    string EmailTo,
    string Name) : IEmailNotification; 