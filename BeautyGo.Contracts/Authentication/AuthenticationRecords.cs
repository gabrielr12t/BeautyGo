namespace BeautyGo.Contracts.Authentication;

public record CurrentUserModel(Guid Id);

public record LoginModel(string Email, string Password);

public record LoginRequest(string Email, string Password);

public record RefreshTokenModel(Guid UserId, string Token, DateTime Expires, DateTime Created, string CreatedByIp, string FingerPrint, bool IsRevoked);

public record TokenResponse(string AccessToken);

public record AuthResponse(string AccessToken, string RefreshToken);

public record RefreshTokenResponse(string RefreshToken);
