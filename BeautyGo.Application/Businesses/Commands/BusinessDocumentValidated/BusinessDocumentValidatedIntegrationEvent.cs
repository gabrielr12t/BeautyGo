using BeautyGo.Application.Core.Abstractions.Messaging;

namespace BeautyGo.Application.Businesses.Commands.BusinessDocumentValidated;

public record BusinessDocumentValidatedIntegrationEvent(Guid BusinessId) : IBusEvent;
