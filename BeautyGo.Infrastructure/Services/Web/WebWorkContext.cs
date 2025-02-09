using BeautyGo.Application.Core.Abstractions.Web;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using BeautyGo.Domain.Common.Defaults;
using BeautyGo.Domain.Helpers;
using System;

namespace BeautyGo.Infrasctructure.Services.Web
{
    internal class WebWorkContext : IWebWorkContext
    {
        private const string SYSTEM_HEADER_KEY = "sys";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseRepository<User> _userRepository;

        public WebWorkContext(
            IHttpContextAccessor httpContextAccessor,
            IBaseRepository<User> userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<User> GetCurrentUserAsync()
        {
            var authenticatedResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            if (!authenticatedResult.Succeeded)
                return null;

            var userId = new Guid(_httpContextAccessor.HttpContext.User?.FindFirstValue("id"));

            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task SetUserTokenCookieAsync(string token, string keySecurity)
        {
            if (_httpContextAccessor.HttpContext?.Response.HasStarted ?? true)
                return;

            var cookieName = $"{BeautyGoCookieDefaults.Prefix}{BeautyGoCookieDefaults.UserTokenCookie}";
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);

            //RECUPERAR DA CONFIG DE AUTH
            var cookieExpires = 2;
            var cookieExpiresDate = DateTime.Now.AddMinutes(cookieExpires);

            if (string.IsNullOrEmpty(token))
                cookieExpiresDate = DateTime.Now.AddMonths(-1);

            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = cookieExpiresDate,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                MaxAge = TimeSpan.FromTicks(cookieExpiresDate.Ticks)
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, await EncryptionHelper.EncryptAsync(token, keySecurity), options);
        }

        public string GetCurrentSystem()
        {
            var hasSystem = _httpContextAccessor.HttpContext.Request.Headers.TryGetValue(SYSTEM_HEADER_KEY, out var system);
            return hasSystem ? system.ToString() : string.Empty;
        }
    }
}
