using BeautyGo.Contracts.Authentication;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Application.Core.Abstractions.Web;

public interface IWebWorkContext
{
    Task<User> GetCurrentUserAsync();

    Task SetUserTokenCookieAsync(string refreshToken, string keySecurity);

    string GetCurrentSystem();
}
