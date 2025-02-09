using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Domain.Entities;

public class AuditEntry : BaseEntity
{
    public string EntityName { get; set; }
    public Guid EntityId { get; set; }

    public Guid? UserId { get; set; }
    public User ModifiedBy { get; set; }

    public string Action { get; set; }
    public DateTime ActionTimestamp { get; set; }

    public string Old { get; set; }
    public string Current { get; set; }

    public string ChangedProperties { get; set; }
}
