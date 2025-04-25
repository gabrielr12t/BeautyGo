namespace BeautyGo.Contracts.Emails;

public record BusinessProfessionalAddedEmail(
    string EmailTo,
    string Professional,
    string Business);
