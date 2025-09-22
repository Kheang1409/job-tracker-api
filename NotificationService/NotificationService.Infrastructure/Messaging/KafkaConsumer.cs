using JobTracker.NotificationService.Domain.Entities;
using JobTracker.NotificationService.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace JobTracker.NotificationService.Infrastructure.Messaging;

public class KafkaConsumer : BackgroundService
{
    private readonly IEmailService _emailService;
    private readonly IConsumer<string, string> _consumer;

    public KafkaConsumer(
        IEmailService emailService,
        IConfiguration configuration
        )
    {
        _emailService = emailService;
        var kafkaConfig = new ConsumerConfig
        {
            BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVERS")
                                ?? configuration["Kafka:BootstrapServers"]
                                ?? throw new InvalidOperationException("Kafka:BootstrapServers is not configured."),
            GroupId = Environment.GetEnvironmentVariable("KAFKA_GROUP_ID")
                        ?? configuration["Kafka:GroupId"]
                        ?? throw new InvalidOperationException("Kafka:GroupId is not configured."),
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _consumer = new ConsumerBuilder<string, string>(kafkaConfig).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(5000, stoppingToken);
        try
        {
            _consumer.Subscribe("job-tracker-topic");

            while (!stoppingToken.IsCancellationRequested)
            {
                var message = _consumer.Consume(stoppingToken);
                if (message?.Message?.Value != null)
                {
                    var payload = JsonConvert.DeserializeObject<dynamic>(message.Message.Value);
                    if (payload is not null)
                    {
                        if (payload.Type.ToString() == "Auth")
                            await HandleResetPassword(payload);
                        if (payload.Type.ToString() == "Quote")
                            await HandleQuote(payload);

                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task HandleResetPassword(dynamic payload)
    {
        var recipient = payload.Email?.ToString();
        var otp = payload.OTP?.ToString();
        var firstName = payload.FirstName?.ToString();
        var notification = ResetPassword.Create(
            recipient,
            "JobTracker Password Reset Request",
            firstName,
            otp
        );

        await _emailService.Send(notification);
    }

    private async Task HandleQuote(dynamic payload)
    {
        var recipient = payload.Email?.ToString();
        var firstName = payload.FirstName?.ToString();
        var quote = payload.Quote?.ToString();
        var author = payload.Author?.ToString();
        var category = payload.Category?.ToString();
        var notification = QuoteResponse.Create(
            recipient,
            $"JobTracker Motivation - {DateTime.UtcNow.ToShortDateString()}",
            firstName,
            quote,
            author,
            category
        );

        await _emailService.Send(notification);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.Close();
        await base.StopAsync(cancellationToken);
    }
}
