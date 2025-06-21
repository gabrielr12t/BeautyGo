using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Domain.DomainEvents;

public record UserConfirmedAccountDomainEvent(User User) : EntityUpdatedDomainEvent<User>(User);

public record UserCreatedDomainEvent(User Entity) : EntityInsertedDomainEvent<User>(Entity);

public record UserIpAddressChangedDomainEvent(User User) : EntityUpdatedDomainEvent<User>(User);

public record UserLoggedinEvent(User Entity) : IDomainEvent;

public record UserNameChangedDomainEvent(User Entity) : EntityUpdatedDomainEvent<User>(Entity);