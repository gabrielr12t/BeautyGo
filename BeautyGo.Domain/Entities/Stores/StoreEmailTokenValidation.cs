using BeautyGo.Domain.Patterns.Visitor.EmailTokenValidation;

namespace BeautyGo.Domain.Entities.Stores;

public class StoreEmailTokenValidation : BeautyGoEmailTokenValidation
{
    public Guid StoreId { get; set; }
    public Store Store { get; set; }

    public override async Task Handle(IEntityValidationTokenHandle visitor) =>
       await visitor.Handle(this);

    public static StoreEmailTokenValidation Create(DateTime expireAt, Guid storeId)
    {
        var token = string.Concat(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

        return new StoreEmailTokenValidation
        {
            Token = token,
            CreatedAt = DateTime.Now,
            ExpiresAt = expireAt,
            StoreId = storeId,
            IsUsed = false
        };
    }
}