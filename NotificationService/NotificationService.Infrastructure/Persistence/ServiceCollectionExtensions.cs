using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using JobTracker.NotificationService.Infrastructure.Extensions;

namespace JobTracker.NotificationService.Infrastructure.Persistence;

public static class ServiceCollectionExtensions
{
     public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services
        .AddMessaging()
        .AddApplicationServices(configuration);

        return services;
    }
}