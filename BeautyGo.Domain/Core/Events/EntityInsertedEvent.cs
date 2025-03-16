using BeautyGo.Domain.Entities;

namespace BeautyGo.Domain.Core.Events;

public record EntityInsertedEvent<T>(T Entity) : IDomainEvent where T : BaseEntity;
