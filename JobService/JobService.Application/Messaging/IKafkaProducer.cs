namespace JobTracker.JobService.Application.Infrastructure.Messaging;
public interface IKafkaProducer
{
    Task Produce<T>(string topic, string key, T value);
}