using BeautyGo.Domain.Patterns.Visitor.EmailTokenValidation;

namespace BeautyGo.Domain.Entities.Users;

public class UserEmailTokenValidation : BeautyGoEmailTokenValidation
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public override async Task Handle(IEntityValidationTokenHandle visitor) =>
        await visitor.Handle(this);

    public static UserEmailTokenValidation Create(DateTime expireAt, Guid userId)
    {
        var token = string.Concat(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

        return new UserEmailTokenValidation
        {
            Token = token,
            CreatedAt = DateTime.Now,
            ExpiresAt = expireAt,
            UserId = userId,
            IsUsed = false
        };
    }
}
