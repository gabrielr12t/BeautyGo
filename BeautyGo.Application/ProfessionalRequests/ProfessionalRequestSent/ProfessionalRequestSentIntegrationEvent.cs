using BeautyGo.Application.Core.Abstractions.Messaging;

namespace BeautyGo.Application.ProfessionalRequests.ProfessionalRequestSent;

public record ProfessionalRequestSentIntegrationEvent(Guid ProfessionalRequestId) : IIntegrationEvent;
