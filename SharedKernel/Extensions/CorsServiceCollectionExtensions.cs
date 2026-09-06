using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobTracker.SharedKernel.Extensions;

public static class CorsServiceCollectionExtensions
{
    public const string PolicyName = "AllowAll";

    public static IServiceCollection AddFrontendCors(
        this IServiceCollection services, IConfiguration configuration)
    {
        var origin = configuration["FRONTEND_ORIGIN"] ?? "http://localhost:4200";
        services.AddCors(options => options.AddPolicy(PolicyName, policy =>
            policy.WithOrigins(origin).AllowAnyHeader().AllowAnyMethod()));
        return services;
    }

    public static WebApplication UseFrontendCors(this WebApplication app)
    {
        app.UseCors(PolicyName);
        return app;
    }
}
