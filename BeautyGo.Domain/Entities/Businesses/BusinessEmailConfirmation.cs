using BeautyGo.Domain.DomainEvents;
using BeautyGo.Domain.Patterns.Visitor.EmailTokenValidation;

namespace BeautyGo.Domain.Entities.Businesses;

public class BusinessEmailConfirmation : EmailConfirmation
{
    public Guid BusinessId { get; set; }
    public virtual Business Business { get; set; }

    public override void Validate()
    {
        Business?.ConfirmAccount();
    }

    public override async Task Handle(IEntityConfirmationHandle visitor) =>
       await visitor.Handle(this);

    public static BusinessEmailConfirmation Create(Guid businessId, DateTime? expireAt = null)
    {
        var token = string.Concat(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

        var entity = new BusinessEmailConfirmation
        {
            Token = token,
            CreatedAt = DateTime.Now,
            ExpiresAt = expireAt ?? DateTime.Now.AddMinutes(10),
            BusinessId = businessId,
            IsUsed = false
        };

        entity.AddDomainEvent(new EmailValidationTokenCreatedEvent(entity));

        return entity;
    }
}