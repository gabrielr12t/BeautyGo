namespace BeautyGo.Contracts.Authentication;

public record TokenModel(
    string AccessToken,
    string RefreshToken);
