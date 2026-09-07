using System.Threading.RateLimiting;

namespace JobTracker.UserService.API.Configuration;

public static class RateLimitingServiceCollectionExtensions
{
    public static IServiceCollection AddApiRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.AddPolicy("authentication", context => CreateLimiter(context, 10, TimeSpan.FromMinutes(1)));
            options.AddPolicy("account-recovery", context => CreateLimiter(context, 5, TimeSpan.FromMinutes(5)));
        });
        return services;
    }

    private static RateLimitPartition<string> CreateLimiter(HttpContext context, int permitLimit, TimeSpan window) =>
        RateLimitPartition.GetFixedWindowLimiter(
            GetClientAddress(context),
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = permitLimit,
                Window = window,
                QueueLimit = 0,
                AutoReplenishment = true
            });

    private static string GetClientAddress(HttpContext context) =>
        context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
        ?? context.Connection.RemoteIpAddress?.ToString()
        ?? "unknown";
}
