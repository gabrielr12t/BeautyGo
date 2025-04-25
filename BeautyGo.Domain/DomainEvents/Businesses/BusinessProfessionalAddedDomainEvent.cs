using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Persons;

namespace BeautyGo.Domain.DomainEvents.Businesses;

public record BusinessProfessionalAddedDomainEvent(Professional Professional) : IDomainEvent;
