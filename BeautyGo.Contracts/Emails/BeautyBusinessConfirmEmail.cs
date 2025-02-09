namespace BeautyGo.Contracts.Emails;

public record BeautyBusinessConfirmEmail(
    string EmailTo,
    string Name,
    string Link);
