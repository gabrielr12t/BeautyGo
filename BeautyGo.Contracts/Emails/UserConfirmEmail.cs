namespace BeautyGo.Contracts.Emails;

public record UserConfirmEmail(
   string EmailTo,
   string Name,
   string Link);
