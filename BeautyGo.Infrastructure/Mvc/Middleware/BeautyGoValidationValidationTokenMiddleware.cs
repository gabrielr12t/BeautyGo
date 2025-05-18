using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Infrastructure.Services.Authentication;
using Microsoft.AspNetCore.Http;

namespace BeautyGo.Infrastructure.Mvc.Middleware;

public class BeautyGoValidationValidationTokenMiddleware
{
    private readonly RequestDelegate _next;

    public BeautyGoValidationValidationTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IAuthService authService)
    {
        var cancellationTokenRequest = context.Request.HttpContext.RequestAborted;

        var token = context.Request.Headers[BeautyGoAuthenticationDefaults.Authorization].FirstOrDefault()?.Split(" ").Last();

        var hasToken = token is not null && !string.IsNullOrWhiteSpace(token);
        if (hasToken && !await authService.IsValidTokenAsync(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next(context);
    }
}
