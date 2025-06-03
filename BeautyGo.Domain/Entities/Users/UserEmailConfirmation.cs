using BeautyGo.Domain.DomainEvents.EmailValidationToken;
using BeautyGo.Domain.Patterns.Visitor.EmailTokenValidation;

namespace BeautyGo.Domain.Entities.Users;

public class UserEmailConfirmation : EmailConfirmation
{
    public Guid UserId { get; set; }
    public virtual User User { get; set; }

    public override void Validate()
    {
        User?.ConfirmAccount();
    }

    public override async Task Handle(IEntityConfirmationHandle visitor) =>
        await visitor.Handle(this);

    public static UserEmailConfirmation Create(Guid userId, DateTime? expireAt = null)
    {
        var token = string.Concat(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

        var entity = new UserEmailConfirmation
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
