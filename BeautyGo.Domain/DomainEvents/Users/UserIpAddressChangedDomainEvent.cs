using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Domain.DomainEvents.Users;

public record UserIpAddressChangedDomainEvent(User User) : IDomainEvent;
