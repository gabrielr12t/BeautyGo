using BeautyGo.Domain.Core.Errors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.RateLimiting;

namespace BeautyGo.Infrastructure.Extensions;

public static class RateLimiterExtensions
{
    public static IServiceCollection AddRateLimiterIp(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = ResponseManyRequests;
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
                httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 15,
                        Window = TimeSpan.FromSeconds(10),
                        QueueLimit = 2
                    }));
        });

        return services;
    }

    private static async ValueTask ResponseManyRequests(OnRejectedContext context, CancellationToken cancellationToken) =>
        await context.HttpContext.Response.WriteAsJsonAsync(DomainErrors.General.ServiceManyRequests, cancellationToken);
}
