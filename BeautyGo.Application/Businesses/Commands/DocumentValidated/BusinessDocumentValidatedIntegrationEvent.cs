using BeautyGo.Application.Core.Abstractions.Messaging;

namespace BeautyGo.Application.Businesses.Commands.DocumentValidated;

public record BusinessDocumentValidatedIntegrationEvent(Guid BusinessId) : IBusEvent;
