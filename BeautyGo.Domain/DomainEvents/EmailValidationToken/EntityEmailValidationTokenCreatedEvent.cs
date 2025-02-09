using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities;

namespace BeautyGo.Domain.DomainEvents.EmailValidationToken;

public class EntityEmailValidationTokenCreatedEvent : IDomainEvent
{
    public EntityEmailValidationTokenCreatedEvent(IEmailValidationToken emailValidationToken) =>
        EmailValidationToken = emailValidationToken;

    public IEmailValidationToken EmailValidationToken { get; set; }
}
