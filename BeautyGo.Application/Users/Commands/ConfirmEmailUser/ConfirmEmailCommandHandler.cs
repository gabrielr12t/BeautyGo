using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications.UserEmailValidationTokens;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Application.Users.Commands.ConfirmEmailUser;

internal class ConfirmEmailCommandHandler
     : ICommandHandler<ConfirmEmailCommand, Result>
{
    #region Fields

    private readonly IBaseRepository<UserEmailTokenValidation> _userEmailValidationTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Ctor

    public ConfirmEmailCommandHandler(IBaseRepository<UserEmailTokenValidation> userEmailValidationTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _userEmailValidationTokenRepository = userEmailValidationTokenRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var userTokenSpec = new UserEmailValidationTokenByTokenSpecification(request.Token)
            .And(new ValidUserEmailValidationTokenSpecification(DateTime.Now))
            .AddInclude(p => p.User);

        var userEmailValidationToken = await _userEmailValidationTokenRepository.GetFirstOrDefaultAsync(userTokenSpec);

        if (userEmailValidationToken is null)
            return Result.Failure(DomainErrors.UserEmailValidationToken.ExpiredToken);

        userEmailValidationToken.MarkTokenAsUsed();

        if (userEmailValidationToken.User is null)
            return Result.Failure(DomainErrors.User.UserNotFound);

        userEmailValidationToken.User.ConfirmEmail();

        _userEmailValidationTokenRepository.Update(userEmailValidationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
