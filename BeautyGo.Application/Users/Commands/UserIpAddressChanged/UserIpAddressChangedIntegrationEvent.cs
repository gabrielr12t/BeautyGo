using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Entities.Users;
using Newtonsoft.Json;

namespace BeautyGo.Application.Users.Commands.UserIpAddressChanged;

public class UserIpAddressChangedIntegrationEvent : IIntegrationEvent
{
    internal UserIpAddressChangedIntegrationEvent(User user) =>
        UserId = user.Id;

    [JsonConstructor]
    private UserIpAddressChangedIntegrationEvent(Guid userId) => UserId = userId;

    public Guid UserId { get; }
}
