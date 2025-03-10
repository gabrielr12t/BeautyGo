using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Entities.Users;
using Newtonsoft.Json;

namespace BeautyGo.Application.Users.Commands.AccountConfirmed;

public class UserConfirmedAccountIntegrationEvent : IIntegrationEvent
{
    internal UserConfirmedAccountIntegrationEvent(User user) => UserId = user.Id;

    [JsonConstructor]
    private UserConfirmedAccountIntegrationEvent(Guid userId) => UserId = userId;

    public Guid UserId { get; }
}
