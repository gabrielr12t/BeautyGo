using BeautyGo.Infrasctructure.Services.Installation;
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
        //var key = staticCache.PrepareKeyForDefaultCache(BeautyGoInstallationDefaults.InstallationAdminInsertedCacheKey, "installed");

        await installation.InstallAsync();

        //await staticCache.GetAsync(key, )

        await _next(context);
    }
}
