using BeautyGo.Domain.Entities;

namespace BeautyGo.Domain.Core.Events;

public class EntityInsertedEvent<T> : IDomainEvent where T : BaseEntity
{
    public EntityInsertedEvent(T entity)
    {
        Entity = entity;
    }

    public T Entity { get; }
}
