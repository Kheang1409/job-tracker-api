using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using JobTracker.JobService.Infrastructure.Extensions;
using JobTracker.SharedKernel.Extensions;

namespace JobTracker.JobService.Infrastructure.Persistence;

public static class ServiceCollectionExtensions
{
     public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddJwtAuthentication(configuration)
            .AddSwaggerDocumentation("Job API")
            .AddMongoDb(configuration)
            .AddMessaging()
            .AddApplicationServices();

        return services;
    }
}