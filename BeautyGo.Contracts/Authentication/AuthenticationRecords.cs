namespace BeautyGo.Contracts.Authentication;

public record CurrentUserModel(Guid Id);

public record LoginModel(string Email, string Password);

public record LoginRequest(string Email, string Password);

public record RefreshTokenModel(Guid UserId, string Token, DateTime Expires, DateTime Created, string CreatedByIp, string FingerPrint, bool IsRevoked);

public record TokenModel(string AccessToken, string RefreshToken);
