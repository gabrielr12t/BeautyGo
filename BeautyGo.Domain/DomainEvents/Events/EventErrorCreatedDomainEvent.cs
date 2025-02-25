using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Events;

namespace BeautyGo.Domain.DomainEvents.Events;

public record EventErrorCreatedDomainEvent(EventError EventError) : IDomainEvent;
