using JobTracker.UserService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JobTracker.UserService.Infrastructure.Extensions;

public static class CronJobServiceCollectionExtensions
{
    public static IServiceCollection AddCronJob(this IServiceCollection services)
    {
        services.AddHttpClient<QuoteFetchingService>();
        services.AddHostedService<QuoteFetchingService>();
        return services;
    }
}