namespace BeautyGo.Contracts.Emails;

public record BusinessConfirmEmail(string EmailTo, string Name, string Link) : IEmailNotification;

public record BusinessProfessionalAddedEmail(string EmailTo, string Professional, string Business) : IEmailNotification;

public record ProfessionalRequestInviteEmail(string EmailTo, string Business) : IEmailNotification;

public record ConfirmAccountEmail(string EmailTo, string Name, string Link) : IEmailNotification;

public record DocumentValidatedEmail(string EmailTo, string BusinessName, string Link) : IEmailNotification;

public record NotificationEmail(string EmailTo, string Subject, string Body) : IEmailNotification;

public record PasswordChangedEmail(string EmailTo, string Name) : IEmailNotification;

public record WelcomeEmail(string EmailTo, string Name) : IEmailNotification;

public record SupportBackgroundFailedEmail(string Subject, string Body): IEmailNotification;

public record ProfessionalRequestReminderEmail(string EmailTo, string Name, string Business, DateTime ExpireIn);
