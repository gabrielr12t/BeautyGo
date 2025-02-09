using BeautyGo.Domain.Entities;

namespace BeautyGo.Domain.Core.Events;

public class EntityDeletedEvent<T> : IDomainEvent where T : BaseEntity
{
    public EntityDeletedEvent(T entity)
    {
        Entity = entity;
    }

    public T Entity { get; }
}
