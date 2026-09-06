using JobTracker.UserService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobTracker.UserService.Infrastructure.Extensions;

public static class EmailServiceCollectionExtensions
{
    public static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<EmailOutboxProtector>();
        services.AddSingleton<EmailServiceClient>();
        services.AddHostedService<EmailOutboxPublisher>();
        return services;
    }
}
