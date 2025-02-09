using BeautyGo.Application.Core.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BeautyGo.Infrastructure.Services.Authentication;

internal sealed class UserIdentifierProvider : IUserIdentifierProvider
{
    public UserIdentifierProvider(IHttpContextAccessor httpContextAccessor)
    {
        string userIdClaim = httpContextAccessor?.HttpContext?.User?.FindFirstValue("id") ?? null;

        if (!string.IsNullOrEmpty(userIdClaim))
            UserId = new Guid(userIdClaim);
    }

    public Guid? UserId { get; }
}