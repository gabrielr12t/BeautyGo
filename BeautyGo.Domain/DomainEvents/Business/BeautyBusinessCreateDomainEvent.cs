using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Business;

namespace BeautyGo.Domain.DomainEvents.Business;

internal record BeautyBusinessCreateDomainEvent(BeautyBusiness Entity) : IDomainEvent;
