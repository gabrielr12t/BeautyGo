using BeautyGo.Application.Core.Abstractions.Messaging;

namespace BeautyGo.Application.ProfessionalRequests.ProfessionalRequestAccepted;

public record ProfessionalRequestAcceptedIntegrationEvent(Guid ProfessionalRequestId) : IIntegrationEvent;
