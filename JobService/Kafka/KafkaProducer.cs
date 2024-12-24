using Confluent.Kafka;
using Newtonsoft.Json;


namespace JobService.Kafka
{
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

        public void Produce<T>(string topic, string key, T value)
        {
            var message = new Message<string, string>
            {
                Key = key,
                Value = JsonConvert.SerializeObject(value)
            };

            try
            {
                _producer.Produce(topic, message, deliveryReport =>
                {
                    if (deliveryReport.Status == PersistenceStatus.NotPersisted)
                    {
                        Console.WriteLine($"Kafka: Failed to deliver message to {topic}.");
                    }
                    else
                    {
                        Console.WriteLine($"Kafka: Delivered message to {topic} with key {key}.");
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kafka Producer Error: {ex.Message}");
            }
        }
    }
}