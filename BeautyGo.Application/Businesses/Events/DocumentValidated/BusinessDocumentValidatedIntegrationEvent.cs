using BeautyGo.Application.Core.Abstractions.Messaging;

namespace BeautyGo.Application.Businesses.Events.DocumentValidated;

public record BusinessDocumentValidatedIntegrationEvent(Guid BusinessId) : IIntegrationEvent;
