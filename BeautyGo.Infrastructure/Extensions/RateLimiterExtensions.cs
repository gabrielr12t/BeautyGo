using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http;

namespace BeautyGo.Infrastructure.Extensions;

public static class RateLimiterExtensions
{
    public static IServiceCollection AddRateLimiterIp(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddPolicy("fixed", httpContext => RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 10,
                    Window = TimeSpan.FromSeconds(5)
                }));
        });

        return services;
    }
}
