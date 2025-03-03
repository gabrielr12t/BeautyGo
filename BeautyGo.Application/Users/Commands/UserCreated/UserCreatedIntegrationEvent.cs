using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Entities.Users;
using Newtonsoft.Json;

namespace BeautyGo.Application.Users.Commands.UserCreated;

public sealed class UserCreatedIntegrationEvent : IBusEvent
{
    internal UserCreatedIntegrationEvent(User userCreatedDomainEvent) =>
        UserId = userCreatedDomainEvent.Id;

    [JsonConstructor]
    private UserCreatedIntegrationEvent(Guid userId) => UserId = userId;

    public Guid UserId { get; }
}
