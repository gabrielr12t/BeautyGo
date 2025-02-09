using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Entities.Users;
using Newtonsoft.Json;

namespace BeautyGo.Application.Users.Events.EmailConfirmed;

public class UserConfirmedEmailIntegrationEvent : IIntegrationEvent
{
    internal UserConfirmedEmailIntegrationEvent(User userCreatedDomainEvent) => UserId = userCreatedDomainEvent.Id;

    [JsonConstructor]
    private UserConfirmedEmailIntegrationEvent(Guid userId) => UserId = userId;

    public Guid UserId { get; }
}
