using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Factories;
using JobTracker.UserService.Infrastructure.Factories;
using JobTracker.UserService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace JobTracker.UserService.Infrastructure.Extensions;

public static class MongoDbServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoConnectionString = configuration.GetConnectionString("MongoDB");
    var databaseName = configuration.GetValue<string>("MongoDbSettings:DatabaseName");

    if (string.IsNullOrEmpty(mongoConnectionString))
        throw new ArgumentException("Connection string 'MongoDB' is not configured.");

    if (string.IsNullOrEmpty(databaseName))
        throw new ArgumentException("MongoDbSettings:DatabaseName is not configured.");

    // Register MongoClient
    services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));

    // Register IMongoDatabase using the configured DB name
    services.AddScoped<IMongoDatabase>(sp =>
    {
        var client = sp.GetRequiredService<IMongoClient>();
        return client.GetDatabase(databaseName);
    });

    // Repositories and Factories
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<ISkillRepository, SkillRepository>();
    services.AddScoped<IAddressRepository, AddressRepository>();
    services.AddScoped<IProjectRepository, ProjectRepository>();

    services.AddScoped<IUserFactory, UserFactory>();

    return services;
    }
}
