using BeautyGo.Domain.Core.Events;

namespace BeautyGo.Domain.Entities;

public interface IEmailValidationToken : IDomainEvent
{
    EmailTokenValidation CreateEmailValidationToken();
}
