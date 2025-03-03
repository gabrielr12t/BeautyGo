using BeautyGo.Domain.DomainEvents.EmailValidationToken;
using BeautyGo.Domain.Patterns.Visitor.EmailTokenValidation;

namespace BeautyGo.Domain.Entities.Users;

public class UserEmailTokenValidation : EmailTokenValidation
{
    public Guid UserId { get; set; }
    public virtual User User { get; set; }

    public override void Validate()
    {
        User?.ConfirmAccount();
    }

    public override async Task Handle(IEntityValidationTokenHandle visitor) =>
        await visitor.Handle(this);

    public static UserEmailTokenValidation Create(Guid userId, DateTime? expireAt = null)
    {
        var token = string.Concat(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

        var entity = new UserEmailTokenValidation
        {
            Token = token,
            CreatedAt = DateTime.Now,
            ExpiresAt = expireAt ?? DateTime.Now.AddMinutes(10),
            UserId = userId,
            IsUsed = false
        };

        entity.AddDomainEvent(new EmailValidationTokenCreatedEvent(entity));

        return entity;
    }
}
