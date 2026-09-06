using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using JobTracker.UserService.Infrastructure.Extensions;
using JobTracker.SharedKernel.Extensions;
using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Factories;
using JobTracker.UserService.Infrastructure.Factories;
using JobTracker.UserService.Infrastructure.Repositories;

namespace JobTracker.UserService.Infrastructure.Persistence;

public static class ServiceCollectionExtensions
{
     public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        MongoMappings.Register();
        services
            .AddJwtAuthentication(configuration)
            .AddSwaggerDocumentation("User API")
            .AddApplicationServices()
            .AddMongoDb(configuration, contextServices => contextServices
                .AddSingleton<IUserRepository, UserRepository>()
                .AddSingleton<IJobApplicationRepository, JobApplicationRepository>()
                .AddScoped<IUserFactory, UserFactory>())
            .AddHostedService<MongoIndexInitializer>()
            .AddEmailServices(configuration);

        return services;
    }
}
