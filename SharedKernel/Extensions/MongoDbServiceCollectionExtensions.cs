using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace JobTracker.SharedKernel.Extensions;

public static class MongoDbServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDb(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IServiceCollection>? registerContextServices = null)
    {
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRINGS")
            ?? configuration.GetConnectionString("MongoDB")
            ?? throw new ArgumentException("Connection string 'MongoDB' is not configured.");
        var databaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME")
            ?? configuration.GetValue<string>("MongoDbSettings:DatabaseName")
            ?? throw new ArgumentException("MongoDbSettings:DatabaseName is not configured.");

        services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));
        services.AddSingleton<IMongoDatabase>(sp =>
            sp.GetRequiredService<IMongoClient>().GetDatabase(databaseName));
        registerContextServices?.Invoke(services);
        return services;
    }
}
