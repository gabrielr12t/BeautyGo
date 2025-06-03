using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities;

namespace BeautyGo.Domain.DomainEvents.EmailValidationToken;

public record EmailValidationTokenCreatedEvent(EmailConfirmation Entity) : IDomainEvent;
