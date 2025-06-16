using JobTracker.NotificationService.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace JobTracker.NotificationService.Infrastructure.Extensions;

public static class MessagingServiceCollectionExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddHostedService<KafkaConsumer>();
        return services;
    }
}