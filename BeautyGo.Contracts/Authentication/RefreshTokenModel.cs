namespace BeautyGo.Contracts.Authentication;

public record RefreshTokenModel(
 Guid UserId,
 string Token,
 DateTime Expires,
 DateTime Created,
 string CreatedByIp, 
 string FingerPrint,
 bool IsRevoked);
