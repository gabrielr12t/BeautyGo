using BeautyGo.Contracts.Authentication;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Application.Core.Abstractions.Authentication;

public interface IAuthService
{
    Task<TokenModel> AuthenticateAsync(User user);

    Task<Result<RefreshTokenModel>> RefreshTokenAsync(Guid userId, string refreshToken);

    Task<User> GetCurrentUserAsync();
}
