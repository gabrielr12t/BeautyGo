using BeautyGo.Domain.Patterns.Visitor.EmailTokenValidation;

namespace BeautyGo.Domain.Entities.Businesses;

public class BusinessEmailTokenValidation : EmailTokenValidation
{
    public Guid BusinessId { get; set; }
    public Business Business { get; set; }

    public override async Task Handle(IEntityValidationTokenHandle visitor) =>
       await visitor.Handle(this);

    public static BusinessEmailTokenValidation Create(DateTime expireAt, Guid businessId)
    {
        var token = string.Concat(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

        return new BusinessEmailTokenValidation
        {
            Token = token,
            CreatedAt = DateTime.Now,
            ExpiresAt = expireAt,
            BusinessId = businessId,
            IsUsed = false
        };
    }
}