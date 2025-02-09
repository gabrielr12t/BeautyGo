using BeautyGo.Contracts.Authentication;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Application.Core.Abstractions.Authentication;

public interface ITokenService
{
    Task<TokenModel> GenerateTokenAsync(User user);

    Task<TokenModel> RefreshTokenAsync(Guid userId);
}
