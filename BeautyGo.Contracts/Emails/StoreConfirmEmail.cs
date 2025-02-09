namespace BeautyGo.Contracts.Emails;

public record StoreConfirmEmail(
    string EmailTo,
    string Name,
    string Link);
