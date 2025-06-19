using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Infrastructure.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BeautyGo.Infrastructure.Mvc.Middleware;

public class BeautyGoValidationTokenMiddleware
{
    private readonly RequestDelegate _next;

    public BeautyGoValidationTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    #region Utilities

    private bool IsTokenStringValid(string token)
    {
        return !string.IsNullOrWhiteSpace(token)
               && !token.Equals("undefined", StringComparison.OrdinalIgnoreCase)
               && !token.Equals("null", StringComparison.OrdinalIgnoreCase);
    }

    private async Task UnauthorizedAsync(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(DomainErrors.General.UnauthorizedUser);
    }

    private bool IsActionAllowAnonymous(HttpContext context)
    {
        return context.GetEndpoint()?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null;
    }

    private string GetTokenFromHeader(HttpContext context)
    {
        return context.Request.Headers[BeautyGoAuthenticationDefaults.Authorization]
                        .FirstOrDefault()?.Split(" ").Last();
    }

    #endregion

    public async Task Invoke(HttpContext context, IAuthService authService)
    {
        var cancellationToken = context.Request.HttpContext.RequestAborted;

        if (IsActionAllowAnonymous(context))
        {
            await _next(context);
            return;
        } 

        if (!IsTokenStringValid(GetTokenFromHeader(context)))
        {
            await UnauthorizedAsync(context);
            return;
        }

        var tokenIsValid = await authService.IsValidTokenAsync(GetTokenFromHeader(context));

        if (!tokenIsValid)
        {
            await UnauthorizedAsync(context);
            return;
        }

        await _next(context);
    }
}
