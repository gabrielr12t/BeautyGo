using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Persons;

namespace BeautyGo.Domain.DomainEvents;

public record BusinessAccountConfirmedDomainEvent(Business Business) : IDomainEvent;

public record BusinessCreatedDomainEvent(Business Business) : IDomainEvent;

public record BusinessDocumentValidatedDomainEvent(Business Business) : IDomainEvent;

public record BusinessProfessionalAddedDomainEvent(Professional Professional) : IDomainEvent;

public record BusinessDocumentConfirmedDomainEvent(Business Business) : IDomainEvent;
