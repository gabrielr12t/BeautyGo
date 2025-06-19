using BeautyGo.Infrastructure.Services.Installation;
using Microsoft.AspNetCore.Http;

namespace BeautyGo.Infrastructure.Mvc.Middleware;

public sealed class InstallationMiddleware
{
    private readonly RequestDelegate _next;

    public InstallationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IInstallationService installation)
    {
        if (!await installation.IsInstalledAsync())
        {
            await installation.InstallAsync();
        }

        await _next(context);
    }
}
