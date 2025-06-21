using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Domain.DomainEvents;

public record UserConfirmedAccountDomainEvent(User User) : IDomainEvent;

public record UserCreatedDomainEvent(User Entity) : IDomainEvent;

public record UserIpAddressChangedDomainEvent(User User) : IDomainEvent;

public record UserLoggedinEvent(User Entity) : IDomainEvent;

public record UserNameChangedDomainEvent(User Entity) : IDomainEvent;