using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using JobTracker.UserService.Infrastructure.Extensions;

namespace JobTracker.UserService.Infrastructure.Persistence;

public static class ServiceCollectionExtensions
{
     public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddJwtAuthentication(configuration)
            .AddSwaggerDocumentation()
            .AddMongoDb(configuration)
            .AddMessaging()
            .AddApplicationServices();

        return services;
    }
}