using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Professionals;

namespace BeautyGo.Domain.DomainEvents;

public record ProfessionalRequestAcceptedDomainEvent(ProfessionalRequest ProfessionalRequest) : IDomainEvent;

public record ProfessionalRequestSentDomainEvent(ProfessionalRequest ProfessionalRequest) : IDomainEvent;
