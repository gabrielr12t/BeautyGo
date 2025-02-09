using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Domain.DomainEvents.Users;

public class UserNameChangedDomainEvent : IDomainEvent
{
    public UserNameChangedDomainEvent(User user) => User = user;

    public User User { get; }
}
