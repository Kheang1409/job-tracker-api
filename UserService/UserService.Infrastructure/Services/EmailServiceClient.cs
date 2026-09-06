using System.Text.Json;
using Confluent.Kafka;
using JobTracker.UserService.Application.Services;
using Microsoft.Extensions.Configuration;

namespace JobTracker.UserService.Infrastructure.Services;

public sealed class EmailServiceClient : IDisposable
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic;

    public EmailServiceClient(IConfiguration configuration)
    {
        var bootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVERS")
            ?? configuration["Kafka:BootstrapServers"] ?? "localhost:9092";
        var username = Environment.GetEnvironmentVariable("KAFKA_USERNAME")
            ?? configuration["Kafka:Username"];
        var password = Environment.GetEnvironmentVariable("KAFKA_PASSWORD")
            ?? configuration["Kafka:Password"];
        _topic = Environment.GetEnvironmentVariable("KAFKA_EMAIL_TOPIC")
            ?? configuration["Kafka:EmailTopic"] ?? "email-notifications";
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = bootstrapServers,
            Acks = Acks.All,
            EnableIdempotence = true
        };
        ConfigureAuthentication(producerConfig, username, password, configuration);
        _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
    }

    public Task PublishAsync(EmailOutboxMessage request, string token, CancellationToken cancellationToken) =>
        _producer.ProduceAsync(_topic, new Message<Null, string>
        {
            Value = JsonSerializer.Serialize(new EmailRequest(
                request.Id, request.Type, request.Recipient, request.FirstName, token))
        }, cancellationToken);

    public void Dispose() => _producer.Dispose();

    private static void ConfigureAuthentication(
        ClientConfig config,
        string? username,
        string? password,
        IConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password))
            return;
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Both KAFKA_USERNAME and KAFKA_PASSWORD are required for authenticated Kafka.");

        var protocol = Environment.GetEnvironmentVariable("KAFKA_SECURITY_PROTOCOL")
            ?? configuration["Kafka:SecurityProtocol"] ?? nameof(SecurityProtocol.SaslSsl);
        var mechanism = Environment.GetEnvironmentVariable("KAFKA_SASL_MECHANISM")
            ?? configuration["Kafka:SaslMechanism"] ?? nameof(SaslMechanism.Plain);
        if (!Enum.TryParse(protocol, true, out SecurityProtocol securityProtocol))
            throw new ArgumentException($"KAFKA_SECURITY_PROTOCOL '{protocol}' is invalid.");
        if (!Enum.TryParse(mechanism, true, out SaslMechanism saslMechanism))
            throw new ArgumentException($"KAFKA_SASL_MECHANISM '{mechanism}' is invalid.");

        config.SecurityProtocol = securityProtocol;
        config.SaslMechanism = saslMechanism;
        config.SaslUsername = username;
        config.SaslPassword = password;
        var caLocation = Environment.GetEnvironmentVariable("KAFKA_SSL_CA_LOCATION")
            ?? configuration["Kafka:SslCaLocation"];
        if (!string.IsNullOrWhiteSpace(caLocation))
            config.SslCaLocation = caLocation;
    }

    private sealed record EmailRequest(string MessageId, string Type, string Recipient, string FirstName, string Token);
}
