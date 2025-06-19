using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Web;
using BeautyGo.Contracts.Authentication;
using BeautyGo.Domain.Caching;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.DomainEvents.Users;
using BeautyGo.Domain.Entities.Persons;
using BeautyGo.Domain.Entities.Security;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Patterns.Specifications.UserRoles;
using BeautyGo.Domain.Providers.Files;
using BeautyGo.Domain.Repositories.Bases;
using BeautyGo.Domain.Settings;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BeautyGo.Infrastructure.Services.Authentication;

public class AuthService : IAuthService
{
    #region Fields

    private readonly IEFBaseRepository<User> _userRepository;
    private readonly IEFBaseRepository<UserRoleMapping> _userRoleRepository;
    private readonly IEFBaseRepository<RefreshToken> _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBeautyGoFileProvider _BeautyGoFileProvider;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IWebHelper _webHelper;
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenService _tokenService;
    private readonly IStaticCacheManager _staticCacheManager;
    private readonly AppSettings _appSettings;
    private User _cachedUser;

    #endregion

    #region Ctor

    public AuthService(
        IEFBaseRepository<User> userRepository,
        AppSettings appSettings,
        IBeautyGoFileProvider BeautyGoFileProvider,
        IHttpContextAccessor contextAccessor,
        IWebHelper webHelper,
        IMediator mediator,
        IEFBaseRepository<UserRoleMapping> userRoleRepository,
        IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor,
        ITokenService tokenService,
        IStaticCacheManager staticCacheManager,
        IEFBaseRepository<RefreshToken> refreshTokenRepository)
    {
        _userRepository = userRepository;
        _appSettings = appSettings;
        _BeautyGoFileProvider = BeautyGoFileProvider;
        _contextAccessor = contextAccessor;
        _webHelper = webHelper;
        _mediator = mediator;
        _userRoleRepository = userRoleRepository;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _tokenService = tokenService;
        _staticCacheManager = staticCacheManager;
        _refreshTokenRepository = refreshTokenRepository;
    }

    #endregion

    #region Utilities

    private async Task<RefreshTokenResponse> GetOrCreateValidRefreshTokenAsync(User user)
    {
        var existingToken = user.RefreshTokens.FirstOrDefault(p => p.IsValid());

        if (existingToken != null && existingToken.IsValid())
            return new(existingToken.Token);

        return await _tokenService.GenerateRefreshTokenAsync(user);
    }

    private async Task<RSA> GetRSAPublicKeyAsync()
    {
        var publicKey = await _BeautyGoFileProvider.ReadAllBytesAsync(_appSettings.Get<SecuritySettings>().PublicKeyFilePath);
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(publicKey, out _);

        return rsa;
    }

    #endregion

    #region Methods

    public async Task<AuthResponse> AuthenticateAsync(User user)
    {
        var refreshTokenResponse = await GetOrCreateValidRefreshTokenAsync(user);
        var tokenResponse = await _tokenService.GenerateTokenAsync(user);

        _cachedUser = user;

        await _mediator.Publish(new UserLoggedinEvent(user));

        return new AuthResponse(tokenResponse.AccessToken, refreshTokenResponse.RefreshToken);
    }

    public async Task<bool> IsValidTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        try
        {
            var rsa = await GetRSAPublicKeyAsync();

            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(rsa)
            };

            var principal = handler.ValidateToken(token, parameters, out _);
            var tokenThumb = principal.FindFirst("ua")?.Value;

            return tokenThumb == _webHelper.GetUserAgent();
        }
        catch
        {
            return false;
        }
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

        var userByIdSpec = new EntityByIdSpecification<User>(userId)
            .AddInclude(q =>
                q.Include(i => i.UserRoles)
                    .ThenInclude(t => t.UserRole));

        _cachedUser = await _userRepository.GetFirstOrDefaultAsync(userByIdSpec, cancellationToken: cancellationToken);

        if (_cachedUser is null)
            return default;

        if (!_cachedUser.IsActive)
            throw new InvalidOperationException($"Usuário '{_cachedUser.Id}' não está ativo.");

        if (_cachedUser.MustChangePassword)
            throw new DomainException(DomainErrors.User.MustChangePassword);

        if (!_cachedUser.EmailConfirmed)
            throw new DomainException(DomainErrors.User.EmailNotConfirmed);

        return _cachedUser;
    }

    public async Task<bool> AuthorizeAsync(string role, CancellationToken cancellationToken = default)
    {
        return await AuthorizeAsync(role, await GetCurrentUserAsync(cancellationToken), cancellationToken);
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

        await _userRepository.RemoveAsync(customer);
        await _userRepository.InsertAsync(owner, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task PromoteCustomerToProfessionalAsync(Customer customer, Guid businessId, CancellationToken cancellationToken)
    {
        var professional = customer.PromoteToProfessional(businessId);

        await _userRepository.RemoveAsync(customer);
        await _userRepository.InsertAsync(professional, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    #endregion 
}
