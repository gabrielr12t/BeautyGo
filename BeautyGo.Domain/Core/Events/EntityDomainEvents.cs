using BeautyGo.Domain.Entities;

namespace BeautyGo.Domain.Core.Events;

public record EntityDeletedDomainEvent<T>(T Entity) : IDomainEvent where T : BaseEntity;
public record EntityInsertedDomainEvent<T>(T Entity) : IDomainEvent where T : BaseEntity;
public record EntityUpdatedDomainEvent<T>(T Entity) : IDomainEvent where T : BaseEntity;
