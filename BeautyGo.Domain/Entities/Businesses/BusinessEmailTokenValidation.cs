using BeautyGo.Domain.DomainEvents.EmailValidationToken;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Visitor.EmailTokenValidation;

namespace BeautyGo.Domain.Entities.Businesses;

public class BusinessEmailTokenValidation : EmailTokenValidation
{
    public Guid BusinessId { get; set; }
    public virtual Business Business { get; set; }

    public override void Validate()
    {
        Business?.ConfirmAccount();
    }

    public override async Task Handle(IEntityValidationTokenHandle visitor) =>
       await visitor.Handle(this);

    public static BusinessEmailTokenValidation Create(Guid businessId, DateTime? expireAt = null)
    {
        var token = string.Concat(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

        var entity = new BusinessEmailTokenValidation
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