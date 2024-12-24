namespace JobService.Kafka
{
    public interface IKafkaProducer
    {
        void Produce<T>(string topic, string key, T value);
    }
}