using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.DomainEvents.EmailValidationToken;
using BeautyGo.Domain.Entities;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Application.EmailValidationToken.EntityEmailValidationTokenCreated;

internal class CreateEmailValidationTokenOnEntityEmailValidationTokenCreatedEventHandler
    : IDomainEventHandler<EntityEmailValidationTokenCreatedEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IBaseRepository<BeautyGoEmailTokenValidation> _emailTokenRepository;

    public CreateEmailValidationTokenOnEntityEmailValidationTokenCreatedEventHandler(
        IBaseRepository<BeautyGoEmailTokenValidation> emailTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _emailTokenRepository = emailTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(EntityEmailValidationTokenCreatedEvent notification, CancellationToken cancellationToken)
    {
        var emailValidationToken = notification.EmailValidationToken.CreateEmailValidationToken();

        await _emailTokenRepository.InsertAsync(emailValidationToken, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
