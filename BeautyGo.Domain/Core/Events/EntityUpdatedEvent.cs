using BeautyGo.Domain.Entities;

namespace BeautyGo.Domain.Core.Events;

public class EntityUpdatedEvent<T> : IDomainEvent where T : BaseEntity
{
    public EntityUpdatedEvent(T entity)
    {
        Entity = entity;
    }

    public T Entity { get; }
}
