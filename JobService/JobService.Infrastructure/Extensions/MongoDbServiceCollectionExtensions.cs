using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Factories;
using JobTracker.JobService.Infrastructure.Factories;
using JobTracker.JobService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace JobTracker.JobService.Infrastructure.Extensions;

public static class MongoDbServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRINGS")
                                    ?? configuration.GetConnectionString("MongoDB")
                                    ?? throw new ArgumentException("Connection string 'MongoDB' is not configured.");
        var databaseName =  Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME")
                            ?? configuration.GetValue<string>("MongoDbSettings:DatabaseName")
                            ?? throw new ArgumentException("MongoDbSettings:DatabaseName is not configured.");

        // Register MongoClient
        services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));

        // Register IMongoDatabase using the configured DB name
        services.AddScoped<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(databaseName);
        });

        // Repositories and Factories
        services.AddScoped<IJobPostRepository, JobPostRepository>();
        services.AddScoped<ISkillRepository, SkillRepository>();
        services.AddScoped<ICandidateRepository, CandidateRepository>();
        services.AddScoped<IJobPostFactory, JobPostFactory>();

        return services;
    }
}
