using JobTracker.SharedKernel.Messaging;
using Microsoft.Extensions.Configuration;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace JobTracker.JobService.Infrastructure.Messaging;
public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducer(IConfiguration configuration)
    {
        var kafkaConfig = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"]
        };
        _producer = new ProducerBuilder<string, string>(kafkaConfig).Build();
    }

    public async Task Produce<T>(string topic, string key, T value)
    {
        var message = new Message<string, string>
        {
            Key = key,
            Value = JsonConvert.SerializeObject(value)
        };

        try
        {
            var deliveryResult = await _producer.ProduceAsync(topic, message);
            if (deliveryResult.Status == PersistenceStatus.NotPersisted)
            {
                Console.WriteLine($"Kafka: Failed to deliver message to {topic}.");
            }
            else
            {
                Console.WriteLine($"Kafka: Delivered message to {deliveryResult.TopicPartitionOffset} with key {key}.");
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}
