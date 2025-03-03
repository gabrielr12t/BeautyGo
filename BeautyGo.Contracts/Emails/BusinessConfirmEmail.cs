namespace BeautyGo.Contracts.Emails;

public record BusinessConfirmEmail(
    string EmailTo,
    string Name,
    string Link) : IEmailNotification;
