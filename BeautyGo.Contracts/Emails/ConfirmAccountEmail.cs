namespace BeautyGo.Contracts.Emails;

public record ConfirmAccountEmail(
   string EmailTo,
   string Name,
   string Link) : IEmailNotification;
