using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Web;
using BeautyGo.Contracts.Authentication;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Repositories.Bases;

namespace BeautyGo.Infrastructure.Services.Authentication;

internal class TokenService : ITokenService
{
    private readonly IEFBaseRepository<User> _userRepository;
    private readonly IWebHelper _webHelper;

    public TokenService(
        IEFBaseRepository<User> userRepository,
        IWebHelper webHelper)
    {
        _userRepository = userRepository;
        _webHelper = webHelper;
    }

    public Task<TokenModel> GenerateTokenAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<TokenModel> RefreshTokenAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}
