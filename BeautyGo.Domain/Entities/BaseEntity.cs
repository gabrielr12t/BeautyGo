using BeautyGo.Domain.Core.Events;

namespace BeautyGo.Domain.Entities;

public abstract class BaseEntity : IEquatable<BaseEntity>
{
    public Guid Id { get; set; }

    public DateTime CreatedOn { get; set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents =>
        _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent @event) =>
        _domainEvents.Add(@event);

    public void RemoveDomainEvent(IDomainEvent @event) =>
        _domainEvents.Remove(@event);

    public void ClearDomainEvents() => _domainEvents.Clear();

    #region Equals

    public static bool operator ==(BaseEntity a, BaseEntity b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(BaseEntity a, BaseEntity b)
        => !(a == b);

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (obj.GetType() != GetType())
            return false;

        if (!(obj is BaseEntity other))
            return false;

        if (Id == Guid.Empty || other.Id == Guid.Empty)
            return false;

        return Id == other.Id;
    }

    public bool Equals(BaseEntity? other)
    {
        if (other is null)
            return false;

        return ReferenceEquals(this, other) || Id == other.Id;
    }

    public override int GetHashCode()
        => Id.GetHashCode() * 41;

    #endregion
}
