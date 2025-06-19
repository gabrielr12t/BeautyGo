using BeautyGo.Domain.Entities.Users;
using System.Security.Cryptography;

namespace BeautyGo.Domain.Entities.Security;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; }
    public string UserAgent { get; set; }
    public string IpAddress { get; set; }
    public DateTime ExpireAt { get; set; }
    public bool IsRevoked { get; set; }

    public User User { get; set; }
    public Guid UserId { get; set; }

    private static string GenerateSecureToken()
    {
        var bytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    public static RefreshToken Create(Guid userId, string userAgent, string ipAddress, DateTime? expireAt = null)
    {
        expireAt ??= DateTime.Now.AddDays(7);

        return new RefreshToken() { UserId = userId, UserAgent = userAgent, IpAddress = ipAddress, Token = GenerateSecureToken(), ExpireAt = expireAt.Value, IsRevoked = false };
    }

    public bool IsValid()
    {
        return !IsRevoked && ExpireAt > DateTime.Now;
    }

    public void Invalidate()
    {
        IsRevoked = true;
        ExpireAt = DateTime.Now;
    }
}
