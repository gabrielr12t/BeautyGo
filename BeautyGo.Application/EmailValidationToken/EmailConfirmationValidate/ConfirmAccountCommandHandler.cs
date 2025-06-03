using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities;
using BeautyGo.Domain.Patterns.Specifications.EmailTokenValidations;
using BeautyGo.Domain.Repositories.Bases;

namespace BeautyGo.Application.EmailValidationToken.EmailTokenValidationValidate;

internal class ConfirmAccountCommandHandler : ICommandHandler<ConfirmAccountCommand, Result>
{
    private readonly IEFBaseRepository<EmailConfirmation> _businessEmailTokenValidationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmAccountCommandHandler(
        IEFBaseRepository<EmailConfirmation> businessEmailTokenValidationRepository,
        IUnitOfWork unitOfWork)
    {
        _businessEmailTokenValidationRepository = businessEmailTokenValidationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ConfirmAccountCommand request, CancellationToken cancellationToken)
    {
        var validTokenSpec = new EmailTokenValidationByTokenSpecification(request.Token)
            .And(new EmailValidationTokenValidSpecification(DateTime.Now));

        var emailValidationToken = await _businessEmailTokenValidationRepository.GetFirstOrDefaultAsync(validTokenSpec, true, cancellationToken);

        if (emailValidationToken is null)
            return Result.Failure(DomainErrors.UserEmailValidationToken.ExpiredToken);

        emailValidationToken.MarkTokenAsUsed();
        emailValidationToken.Validate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
