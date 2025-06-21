using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities;

namespace BeautyGo.Domain.DomainEvents;

public record EmailValidationTokenCreatedEvent(EmailConfirmation Entity) : EntityInsertedDomainEvent<EmailConfirmation>(Entity);
