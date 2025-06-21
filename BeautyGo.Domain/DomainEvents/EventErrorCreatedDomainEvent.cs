using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Events;

namespace BeautyGo.Domain.DomainEvents;

public record EventErrorCreatedDomainEvent(EventError EventError) : EntityInsertedDomainEvent<EventError>(EventError);
