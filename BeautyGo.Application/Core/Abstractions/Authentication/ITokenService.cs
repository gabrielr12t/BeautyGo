using BeautyGo.Contracts.Authentication;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Application.Core.Abstractions.Authentication;

public interface ITokenService
{
    Task<TokenResponse> GenerateTokenAsync(User user);

    Task<RefreshTokenResponse> GenerateRefreshTokenAsync(User user, CancellationToken cancellationToken = default);
}
