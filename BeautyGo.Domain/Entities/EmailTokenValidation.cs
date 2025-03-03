using BeautyGo.Domain.Patterns.Visitor.EmailTokenValidation;

namespace BeautyGo.Domain.Entities;

public abstract class EmailTokenValidation : BaseEntity
{
    public string Token { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }

    public virtual void MarkTokenAsUsed()
    {
        IsUsed = true;
        ExpiresAt = DateTime.Now.AddDays(-1);
    }

    public abstract void Validate();

    public abstract Task Handle(IEntityValidationTokenHandle visitor);

    public void GenerateNewToken() =>
        string.Concat(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));
}
