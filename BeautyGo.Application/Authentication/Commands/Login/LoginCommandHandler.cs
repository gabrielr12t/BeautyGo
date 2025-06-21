using BeautyGo.Application.Authentication.Caching;
using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Contracts.Authentication;
using BeautyGo.Domain.Caching;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Helpers;
using BeautyGo.Domain.Patterns.Specifications.UserEmailConfirmations;
using BeautyGo.Domain.Patterns.Specifications.Users;
using BeautyGo.Domain.Repositories.Bases;
using Microsoft.EntityFrameworkCore;

namespace BeautyGo.Application.Authentication.Commands.Login;

internal class LoginCommandHandler : ICommandHandler<LoginCommand, Result<AuthResponse>>
{
    #region Fields

    private readonly IEFBaseRepository<User> _userRepository;
    private readonly IEFBaseRepository<UserEmailConfirmation> _userEmailConfirmationRepository;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStaticCacheManager _staticCacheManager;

    #endregion

    #region Ctor

    public LoginCommandHandler(
        IEFBaseRepository<User> userRepository,
        IEFBaseRepository<UserEmailConfirmation> userEmailValidationTokenRepository,
        IAuthService authService,
        IUnitOfWork unitOfWork,
        IStaticCacheManager staticCacheManager)
    {
        _userRepository = userRepository;
        _userEmailConfirmationRepository = userEmailValidationTokenRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
        _staticCacheManager = staticCacheManager;
    }

    #endregion

    #region Utilities

    private async Task<bool> HasEmailTokenPendingValidationAsync(User user, CancellationToken cancellationToken)
    {
        var userEmailValidationTokenValid = new UserEmailConfirmationByUserIdSpecification(user.Id)
            .And(new ValidUserEmailConfirmationSpecification(DateTime.Now));

        return await _userEmailConfirmationRepository.ExistAsync(userEmailValidationTokenValid);
    }

    private async Task CreateNewUserEmailValidationTokenAsync(User user, CancellationToken cancellationToken)
    {
        var userEmailToken = UserEmailConfirmation.Create(user.Id);
        await _userEmailConfirmationRepository.InsertAsync(userEmailToken, cancellationToken);
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

    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var userByEmailSpec = new UserByEmailSpecification(request.Email)
            .AddInclude(
                p => p.Include(x => x.Passwords),
                p => p.Include(x => x.RefreshTokens),
                p => p.Include(x => x.UserRoles)
                    .ThenInclude(t => t.UserRole)
            );

        //var cacheKey = _staticCacheManager.PrepareKeyForShortTermCache(BeautyGoAuthenticationDefaults.UserByUserEmail, request.Email);

        //var user = await _staticCacheManager.GetAsync(cacheKey, async () => await _userRepository.GetFirstOrDefaultAsync(userByEmailSpec, true, cancellationToken));

        var user = await _userRepository.GetFirstOrDefaultAsync(userByEmailSpec, true, cancellationToken);

        if (user == null)
            return Result.Failure<AuthResponse>(DomainErrors.User.UserNotFound);

        if (!PasswordsMatch(user.GetCurrentPassword(), request.Password))
            return Result.Failure<AuthResponse>(DomainErrors.Authentication.InvalidEmailOrPassword);

        if (!user.IsActive)
            return Result.Failure<AuthResponse>(DomainErrors.User.UserNotActive);

        if (!user.EmailConfirmed && !await HasEmailTokenPendingValidationAsync(user, cancellationToken))
            await CreateNewUserEmailValidationTokenAsync(user, cancellationToken);

        if (!user.EmailConfirmed)
            return Result.Failure<AuthResponse>(DomainErrors.UserEmailValidationToken.RequiredValidToken);

        if (user.MustChangePassword)
            return Result.Failure<AuthResponse>(DomainErrors.User.MustChangePassword);

        user.LastLoginDate = DateTime.Now;
        user.LastActivityDate = DateTime.Now;

        var token = await _authService.AuthenticateAsync(user);

        await _userRepository.UpdateAsync(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(token);
    }
}
