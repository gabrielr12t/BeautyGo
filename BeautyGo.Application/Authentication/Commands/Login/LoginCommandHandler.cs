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

    private readonly IBaseRepository<UserRole> _roleRepository;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<UserEmailTokenValidation> _userEmailValidationTokenRepository;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Ctor

    public LoginCommandHandler(IBaseRepository<UserRole> roleRepository,
        IBaseRepository<User> userRepository,
        IBaseRepository<UserEmailTokenValidation> userEmailValidationTokenRepository,
        IAuthService authService,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _userEmailValidationTokenRepository = userEmailValidationTokenRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Utilities

    private async Task<User?> GetUserWithSpecificationsAsync(string email)
    {
        var userByEmailSpec = new UserByEmailSpecification(email)
            .AddInclude(p => p.Passwords)
            .AddInclude(p => p.UserRoles)
            .AddInclude($"{nameof(User.UserRoles)}.{nameof(UserRoleMapping.UserRole)}");

        return await _userRepository.GetFirstOrDefaultAsync(userByEmailSpec);
    }

    private Result ValidateUserState(User user)
    {
        if (!user.IsActive)
            return Result.Failure(DomainErrors.User.UserNotActive);

        return Result.Success();
    }

    private async Task<Result> HandleEmailValidationAsync(User user, CancellationToken cancellationToken)
    {
        if (user.EmailConfirmed)
            return Result.Success();

        var userTokenSpec = new UserEmailValidationTokenByUserIdSpecification(user.Id)
            .And(new ValidUserEmailValidationTokenSpecification(DateTime.Now));

        var hasValidToken = await _userEmailValidationTokenRepository.ExistAsync(userTokenSpec);

        if (!hasValidToken)
        {
            var userEmailToken = UserEmailTokenValidation.Create(DateTime.Now.AddMinutes(3), user.Id);
            await _userEmailValidationTokenRepository.InsertAsync(userEmailToken, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Failure(DomainErrors.UserEmailValidationToken.NewToken);
        }

        return Result.Success();
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
        var user = await GetUserWithSpecificationsAsync(request.Email);
        if (user == null)
            return Result.Failure<TokenModel>(DomainErrors.User.UserNotFound);

        if (!PasswordsMatch(user.GetCurrentPassword(), request.Password))
            return Result.Failure<TokenModel>(DomainErrors.Authentication.InvalidEmailOrPassword);

        if (!user.IsActive)
            return Result.Failure<TokenModel>(DomainErrors.User.UserNotActive);

        var emailValidationResult = await HandleEmailValidationAsync(user, cancellationToken);
        if (!emailValidationResult.IsSuccess)
            return Result.Failure<TokenModel>(emailValidationResult.Error);

        //VALIDAR AQUI MUITAS TENTATIVAS

        user.LastLoginDate = DateTime.Now;

        var token = await _authService.AuthenticateAsync(user);
        return Result.Success(token);
    }
}
