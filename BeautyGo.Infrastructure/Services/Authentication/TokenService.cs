using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Web;
using BeautyGo.Contracts.Authentication;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Entities.Security;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Providers.Files;
using BeautyGo.Domain.Repositories.Bases;
using BeautyGo.Domain.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BeautyGo.Infrastructure.Services.Authentication;

internal class TokenService : ITokenService
{
    #region Fields

    private readonly IEFBaseRepository<RefreshToken> _refreshTokenRepository;

    private readonly IWebHelper _webHelper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBeautyGoFileProvider _BeautyGoFileProvider;
    private readonly AppSettings _appSettings;

    #endregion

    #region Ctor

    public TokenService(
        IEFBaseRepository<User> userRepository,
        IEFBaseRepository<RefreshToken> refreshTokenRepository,
        IWebHelper webHelper,
        IBeautyGoFileProvider beautyGoFileProvider,
        AppSettings appSettings,
        IUnitOfWork unitOfWork)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _webHelper = webHelper;
        _BeautyGoFileProvider = beautyGoFileProvider;
        _appSettings = appSettings;
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
            new("ua", _webHelper.GetUserAgent())
        };

        claims.AddRange(user.UserRoles.Select(role => new Claim(ClaimTypes.Role, role.UserRole.Description)));

        return claims;
    }

    private async Task<RSA> GetRSAPrivateKeyAsync()
    {
        var privateKey = await _BeautyGoFileProvider.ReadAllBytesAsync(_appSettings.Get<SecuritySettings>().PrivateKeyFilePath);
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(privateKey, out _);

        return rsa;
    }

    #endregion

    public async Task<TokenResponse> GenerateTokenAsync(User user)
    {
        var claims = GenerateUserClaims(user);

        var rsa = await GetRSAPrivateKeyAsync();

        var credentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

        var authSettings = _appSettings.Get<AuthSettings>();

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(authSettings.ExpirationTokenMinutes),
            Issuer = authSettings.Issuer,
            Audience = authSettings.Audience,
            NotBefore = DateTime.UtcNow,
            SigningCredentials = credentials,
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new(tokenHandler.WriteToken(token));
    }

    public async Task<RefreshTokenResponse> GenerateRefreshTokenAsync(User user)
    {
        var refreshToken = RefreshToken.Create(user.Id, _webHelper.GetUserAgent(), await _webHelper.GetCurrentIpAddressAsync());

        await _refreshTokenRepository.InsertAsync(refreshToken);

        await _unitOfWork.SaveChangesAsync();

        return new RefreshTokenResponse(refreshToken.Token);
    }
}
