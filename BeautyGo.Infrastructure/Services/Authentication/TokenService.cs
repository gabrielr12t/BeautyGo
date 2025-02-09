using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Web;
using BeautyGo.Contracts.Authentication;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Infrasctructure.Services.Authentication;

internal class TokenService : ITokenService
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IWebHelper _webHelper;

    public TokenService(
        IBaseRepository<User> userRepository, 
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
