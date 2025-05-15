using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Web;
using BeautyGo.Contracts.Authentication;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.DomainEvents.Users;
using BeautyGo.Domain.Entities.Persons;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Patterns.Specifications.UserRoles;
using BeautyGo.Domain.Patterns.Specifications.Users;
using BeautyGo.Domain.Providers.Files;
using BeautyGo.Domain.Repositories;
using BeautyGo.Domain.Settings;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BeautyGo.Infrastructure.Services.Authentication;

public class AuthService : IAuthService
{
    #region Fields

    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<UserRoleMapping> _userRoleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBeautyGoFileProvider _BeautyGoFileProvider;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IWebHelper _webHelper;
    private readonly IWebWorkContext _webWorkContext;
    private readonly IMediator _mediator;
    private readonly AppSettings _appSettings;
    private User _cachedUser;

    #endregion

    #region Ctor

    public AuthService(
        IBaseRepository<User> userRepository,
        AppSettings appSettings,
        IBeautyGoFileProvider BeautyGoFileProvider,
        IHttpContextAccessor contextAccessor,
        IWebHelper webHelper,
        IWebWorkContext webWorkContext,
        IMediator mediator,
        IBaseRepository<UserRoleMapping> userRoleRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _appSettings = appSettings;
        _BeautyGoFileProvider = BeautyGoFileProvider;
        _contextAccessor = contextAccessor;
        _webHelper = webHelper;
        _webWorkContext = webWorkContext;
        _mediator = mediator;
        _userRoleRepository = userRoleRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Utilities

    private List<Claim> GenerateUserClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString(), ClaimValueTypes.String, BeautyGoAuthenticationDefaults.ClaimsIssuer),
            new(ClaimTypes.Role, string.Join(",", user.UserRoles.Select(p => p.UserRole.Description))),
        };

        claims.AddRange(user.UserRoles.Select(role => new Claim(ClaimTypes.Role, role.UserRole.Description)));

        return claims;
    }

    private async Task<RSA> GetRsaKeyAsync()
    {
        var rsa = RSA.Create();
        var key = await _BeautyGoFileProvider.ReadAllTextAsync(
            _appSettings.Get<SecuritySettings>().PrivateKeyFilePath,
            Encoding.UTF8);

        rsa.FromXmlString(key);
        return rsa;
    }

    private async Task<string> GenerateJwtTokenAsync(User user)
    {
        var claims = GenerateUserClaims(user);

        var rsa = await GetRsaKeyAsync();

        var credentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

        var authSettings = _appSettings.Get<AuthSettings>();

        var tokenDescriptor = new JwtSecurityToken(
            issuer: authSettings.Issuer,
            audience: authSettings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(authSettings.ExpirationTokenMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    private async Task<RefreshTokenModel> GenerateRefreshTokenAsync(User user)
    {
        var refreshToken = new RefreshTokenModel(
            user.Id,
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            DateTime.UtcNow.AddDays(7),
            DateTime.UtcNow,
            await _webHelper.GetCurrentIpAddressAsync(),
            _webHelper.GetUserAgent(),
            true);

        return refreshToken;
    }

    #endregion

    #region Methods

    public async Task<TokenModel> AuthenticateAsync(User user)
    {
        var token = await GenerateJwtTokenAsync(user);
        var refreshToken = await GenerateRefreshTokenAsync(user);

        _cachedUser = user;

        await _mediator.Publish(new UserLoggedinEvent(user));

        return new TokenModel(token, refreshToken.Token);
    }

    public async Task<Result<RefreshTokenModel>> RefreshTokenAsync(Guid userId, string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
            return Result.Failure<RefreshTokenModel>(DomainErrors.Authentication.InvalidRefreshToken);

        ///AJUSTAR
        RefreshTokenModel businessRefreshToken = null; //await _staticCacheManager.GetAsync<RefreshTokenModel>(cacheKey);
        if (businessRefreshToken == null)
            return Result.Failure<RefreshTokenModel>(DomainErrors.Authentication.InvalidRefreshToken);

        if (businessRefreshToken.IsRevoked)
            return Result.Failure<RefreshTokenModel>(DomainErrors.Authentication.InvalidRefreshToken);

        if (_webHelper.GetUserAgent() != businessRefreshToken.FingerPrint)
            return Result.Failure<RefreshTokenModel>(DomainErrors.Authentication.InvalidRefreshToken);

        var user = await _userRepository.GetByIdAsync(userId);
        var newRefreshToken = await GenerateRefreshTokenAsync(user);

        return newRefreshToken;
    }

    //AJUSTAR PARA RETORNAR RESULT
    public async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        if (_cachedUser != null)
            return _cachedUser;

        if (_contextAccessor is null || _contextAccessor.HttpContext is null)
            return null;

        var authenticateResult = await _contextAccessor.HttpContext.AuthenticateAsync(BeautyGoAuthenticationDefaults.AuthenticationScheme);

        if (!authenticateResult.Succeeded)
            return null;

        var userId = new Guid(authenticateResult.Principal.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value);

        _cachedUser = await _userRepository.GetFirstOrDefaultAsync(
            new EntityByIdSpecification<User>(userId).And(
                new UserWithRolesSpecification()), cancellationToken: cancellationToken);

        if (_cachedUser is null)
            return default;

        if (!_cachedUser.IsActive)
            throw new InvalidOperationException($"Usuário '{_cachedUser.Id}' não está ativo.");

        return _cachedUser;
    }

    public async Task<bool> AuthorizeAsync(string role, CancellationToken cancellationToken = default)
    {
        return await AuthorizeAsync(role, await GetCurrentUserAsync(cancellationToken));
    }

    public async Task<bool> AuthorizeAsync(string role, User user, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(role))
            return false;

        var userRoleSpecification = new UserRoleByUserIdSpecification(user.Id);
        var userRoles = await _userRoleRepository.GetAsync(userRoleSpecification, cancellationToken: cancellationToken);

        return userRoles.Any(p => string.Equals(p.UserRole.Description, role, StringComparison.InvariantCultureIgnoreCase));
    }

    public async Task PromoteCustomerToOwnerAsync(Customer customer, CancellationToken cancellationToken)
    {
        var owner = customer.PromoteToOwner();

        _userRepository.Remove(customer);
        await _userRepository.InsertAsync(owner, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task PromoteCustomerToProfessionalAsync(Customer customer, Guid businessId, CancellationToken cancellationToken)
    {
        var professional = customer.PromoteToProfessional(businessId);

        _userRepository.Remove(customer);
        await _userRepository.InsertAsync(professional, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    #endregion 
}
