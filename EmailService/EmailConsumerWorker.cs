using System.Text.Json;
using Confluent.Kafka;
using MailKit.Security;
using System.Text;

namespace JobTracker.EmailService;

public sealed class EmailConsumerWorker(IConfiguration configuration, EmailSender emailSender, ILogger<EmailConsumerWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var bootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVERS") ?? configuration["Kafka:BootstrapServers"] ?? "localhost:9092";
        var topic = Environment.GetEnvironmentVariable("KAFKA_EMAIL_TOPIC") ?? configuration["Kafka:EmailTopic"] ?? "email-notifications";
        var groupId = Environment.GetEnvironmentVariable("KAFKA_EMAIL_CONSUMER_GROUP") ?? configuration["Kafka:ConsumerGroup"] ?? "email-service";
        var deadLetterTopic = Environment.GetEnvironmentVariable("KAFKA_EMAIL_DLQ_TOPIC") ?? configuration["Kafka:DeadLetterTopic"] ?? $"{topic}.dead-letter";
        var maxDeliveryAttempts = int.TryParse(Environment.GetEnvironmentVariable("EMAIL_MAX_DELIVERY_ATTEMPTS"), out var configuredAttempts)
            ? Math.Max(configuredAttempts, 1)
            : 5;
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };
        ConfigureAuthentication(consumerConfig, configuration);
        using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        var producerConfig = new ProducerConfig { BootstrapServers = bootstrapServers, Acks = Acks.All, EnableIdempotence = true };
        ConfigureAuthentication(producerConfig, configuration);
        using var deadLetterProducer = new ProducerBuilder<Null, string>(producerConfig).Build();
        var deliveryAttempts = new Dictionary<TopicPartitionOffset, int>();
        consumer.Subscribe(topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ConsumeResult<Ignore, string> result;
                try
                {
                    result = consumer.Consume(stoppingToken);
                }
                catch (ConsumeException exception)
                {
                    logger.LogError(exception, "Kafka consume failed; retrying.");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                    continue;
                }

                EmailRequest? request;
                try
                {
                    request = JsonSerializer.Deserialize<EmailRequest>(result.Message.Value) ?? throw new JsonException("Email message was empty.");
                    if (request.Type is not ("verification" or "password-reset"))
                        throw new JsonException($"Unsupported email type '{request.Type}'.");
                }
                catch (JsonException exception)
                {
                    logger.LogError(exception, "Discarding invalid email message at {Offset}.", result.TopicPartitionOffset);
                    consumer.Commit(result);
                    deliveryAttempts.Remove(result.TopicPartitionOffset);
                    continue;
                }

                try
                {
                    await emailSender.SendAsync(request, stoppingToken);
                    consumer.Commit(result);
                    deliveryAttempts.Remove(result.TopicPartitionOffset);
                    logger.LogInformation("Sent {EmailType} email {MessageId} to {Recipient}.", request.Type, request.MessageId, request.Recipient);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (AuthenticationException exception)
                {
                    logger.LogCritical(
                        exception,
                        "SMTP authentication failed. Update SMTP_SENDER_EMAIL and SMTP_SENDER_PASSWORD; retrying in 5 minutes.");
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                    consumer.Seek(result.TopicPartitionOffset);
                }
                catch (Exception exception)
                {
                    var attempt = deliveryAttempts.GetValueOrDefault(result.TopicPartitionOffset) + 1;
                    deliveryAttempts[result.TopicPartitionOffset] = attempt;
                    if (attempt >= maxDeliveryAttempts)
                    {
                        try
                        {
                            await deadLetterProducer.ProduceAsync(deadLetterTopic, new Message<Null, string>
                            {
                                Value = result.Message.Value,
                                Headers = new Headers
                                {
                                    { "source-topic", Encoding.UTF8.GetBytes(result.Topic) },
                                    { "source-offset", Encoding.UTF8.GetBytes(result.Offset.Value.ToString()) },
                                    { "failure", Encoding.UTF8.GetBytes(exception.GetType().Name) }
                                }
                            }, stoppingToken);
                            consumer.Commit(result);
                            deliveryAttempts.Remove(result.TopicPartitionOffset);
                            logger.LogError(exception, "Email delivery exhausted {AttemptCount} attempts; moved message to {DeadLetterTopic}.", attempt, deadLetterTopic);
                        }
                        catch (Exception deadLetterException) when (deadLetterException is not OperationCanceledException)
                        {
                            logger.LogError(deadLetterException, "Could not publish failed email to {DeadLetterTopic}; retaining the source message.", deadLetterTopic);
                            consumer.Seek(result.TopicPartitionOffset);
                        }
                        continue;
                    }

                    var retryDelay = TimeSpan.FromSeconds(Math.Min(Math.Pow(2, attempt), 30));
                    logger.LogError(exception, "Email delivery attempt {AttemptCount} failed; retrying in {RetryDelay}.", attempt, retryDelay);
                    await Task.Delay(retryDelay, stoppingToken);
                    consumer.Seek(result.TopicPartitionOffset);
                }
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { }
        finally { consumer.Close(); }
    }

    private static void ConfigureAuthentication(ClientConfig config, IConfiguration configuration)
    {
        var username = Environment.GetEnvironmentVariable("KAFKA_USERNAME") ?? configuration["Kafka:Username"];
        var password = Environment.GetEnvironmentVariable("KAFKA_PASSWORD") ?? configuration["Kafka:Password"];
        if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password))
            return;
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Both KAFKA_USERNAME and KAFKA_PASSWORD are required for authenticated Kafka.");

        var protocol = Environment.GetEnvironmentVariable("KAFKA_SECURITY_PROTOCOL")
            ?? configuration["Kafka:SecurityProtocol"] ?? nameof(SecurityProtocol.SaslSsl);
        var mechanism = Environment.GetEnvironmentVariable("KAFKA_SASL_MECHANISM")
            ?? configuration["Kafka:SaslMechanism"] ?? nameof(Confluent.Kafka.SaslMechanism.Plain);
        if (!Enum.TryParse(protocol, true, out SecurityProtocol securityProtocol))
            throw new ArgumentException($"KAFKA_SECURITY_PROTOCOL '{protocol}' is invalid.");
        if (!Enum.TryParse(mechanism, true, out Confluent.Kafka.SaslMechanism saslMechanism))
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
}
