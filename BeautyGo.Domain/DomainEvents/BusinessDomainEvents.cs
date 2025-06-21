using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Persons;

namespace BeautyGo.Domain.DomainEvents;

public record BusinessAccountConfirmedDomainEvent(Business Business) : EntityUpdatedDomainEvent<Business>(Business);

public record BusinessCreatedDomainEvent(Business Business) : EntityInsertedDomainEvent<Business>(Business);

public record BusinessDocumentValidatedDomainEvent(Business Business) : EntityUpdatedDomainEvent<Business>(Business);

public record BusinessProfessionalAddedDomainEvent(Professional Professional) : EntityUpdatedDomainEvent<Professional>(Professional);

public record BusinessDocumentConfirmedDomainEvent(Business Business) : EntityUpdatedDomainEvent<Business>(Business);
