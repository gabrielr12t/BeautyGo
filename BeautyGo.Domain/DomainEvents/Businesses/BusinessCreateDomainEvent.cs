using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Businesses;

namespace BeautyGo.Domain.DomainEvents.Businesses;

internal record BusinessCreateDomainEvent(Business Entity) : IDomainEvent;
