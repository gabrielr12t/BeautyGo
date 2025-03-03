namespace BeautyGo.Contracts.Emails;

public record PasswordChangedEmail(
    string EmailTo,
    string Name) : IEmailNotification; 
