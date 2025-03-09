using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Contracts.Authentication;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Helpers;
using BeautyGo.Domain.Patterns.Specifications.UserEmailValidationTokens;
using BeautyGo.Domain.Patterns.Specifications.Users;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Application.Authentication.Commands.Login;

internal class LoginCommandHandler : ICommandHandler<LoginCommand, Result<TokenModel>>
{
    #region Fields

    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<UserEmailTokenValidation> _userEmailValidationTokenRepository;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Ctor

    public LoginCommandHandler(
        IBaseRepository<User> userRepository,
        IBaseRepository<UserEmailTokenValidation> userEmailValidationTokenRepository,
        IAuthService authService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _userEmailValidationTokenRepository = userEmailValidationTokenRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Utilities

    private async Task<bool> HasEmailTokenPendingValidationAsync(User user, CancellationToken cancellationToken)
    {
        var userEmailValidationTokenValid = new UserEmailValidationTokenByUserIdSpecification(user.Id)
            .And(new ValidUserEmailValidationTokenSpecification(DateTime.Now));

        return await _userEmailValidationTokenRepository.ExistAsync(userEmailValidationTokenValid);
    }

    private async Task CreateNewUserEmailValidationTokenAsync(User user, CancellationToken cancellationToken)
    {
        var userEmailToken = UserEmailTokenValidation.Create(user.Id);
        await _userEmailValidationTokenRepository.InsertAsync(userEmailToken, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private bool PasswordsMatch(UserPassword userPassword, string enteredPassword)
    {
        if (string.IsNullOrWhiteSpace(userPassword?.Password) || string.IsNullOrWhiteSpace(enteredPassword))
            return false;

        var hashedEnteredPassword = EncryptionHelper.CreatePasswordHash(enteredPassword, userPassword.Salt);
        return string.Equals(userPassword.Password, hashedEnteredPassword, StringComparison.Ordinal);
    }

    #endregion

    public async Task<Result<TokenModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var userByEmailSpec = new UserByEmailSpecification(request.Email).AddInclude(p => p.Passwords);

        var user = await _userRepository.GetFirstOrDefaultAsync(userByEmailSpec);
        if (user == null)
            return Result.Failure<TokenModel>(DomainErrors.User.UserNotFound);

        if (!PasswordsMatch(user.GetCurrentPassword(), request.Password))
            return Result.Failure<TokenModel>(DomainErrors.Authentication.InvalidEmailOrPassword);

        if (!user.IsActive)
            return Result.Failure<TokenModel>(DomainErrors.User.UserNotActive);

        if (!user.EmailConfirmed && !await HasEmailTokenPendingValidationAsync(user, cancellationToken))
            await CreateNewUserEmailValidationTokenAsync(user, cancellationToken);

        if (!user.EmailConfirmed)
            return Result.Failure<TokenModel>(DomainErrors.UserEmailValidationToken.RequiredValidToken);

        user.LastLoginDate = DateTime.Now;

        var token = await _authService.AuthenticateAsync(user);

        _userRepository.Update(user);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success(token);
    }
}
