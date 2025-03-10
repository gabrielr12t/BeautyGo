namespace BeautyGo.Application.Core.Abstractions.Messaging;

public record Message(Ulid Id, IIntegrationEvent IntegrationEvent);
