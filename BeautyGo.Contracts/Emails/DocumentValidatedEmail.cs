namespace BeautyGo.Contracts.Emails;

public record DocumentValidatedEmail(
    string EmailTo,
    string BusinessName,
    string Link);
