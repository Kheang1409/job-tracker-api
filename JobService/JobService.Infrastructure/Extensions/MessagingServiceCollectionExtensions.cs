using JobTracker.SharedKernel.Messaging;
using JobTracker.JobService.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace JobTracker.JobService.Infrastructure.Extensions;

public static class MessagingServiceCollectionExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddSingleton<IKafkaProducer, KafkaProducer>();
        return services;
    }
}