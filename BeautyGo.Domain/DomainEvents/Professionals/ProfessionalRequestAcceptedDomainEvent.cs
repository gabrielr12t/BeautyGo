using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Professionals;

namespace BeautyGo.Domain.DomainEvents.Professionals;

public record ProfessionalRequestAcceptedDomainEvent(ProfessionalRequest ProfessionalRequest) : IDomainEvent;
