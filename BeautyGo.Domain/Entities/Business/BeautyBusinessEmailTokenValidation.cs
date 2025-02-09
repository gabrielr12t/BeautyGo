using BeautyGo.Domain.Patterns.Visitor.EmailTokenValidation;

namespace BeautyGo.Domain.Entities.Business;

public class BeautyBusinessEmailTokenValidation : BeautyGoEmailTokenValidation
{
    public Guid BusinessId { get; set; }
    public BeautyBusiness Business { get; set; }

    public override async Task Handle(IEntityValidationTokenHandle visitor) =>
       await visitor.Handle(this);

    public static BeautyBusinessEmailTokenValidation Create(DateTime expireAt, Guid businessId)
    {
        var token = string.Concat(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

        return new BeautyBusinessEmailTokenValidation
        {
            Token = token,
            CreatedAt = DateTime.Now,
            ExpiresAt = expireAt,
            BusinessId = businessId,
            IsUsed = false
        };
    }
}