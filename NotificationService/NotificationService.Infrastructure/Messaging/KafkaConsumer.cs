using JobTracker.NotificationService.Domain.Entities;
using JobTracker.NotificationService.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace JobTracker.NotificationService.Infrastructure.Messaging;

public class KafkaConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEmailService _emailService;
    private readonly IConsumer<string, string> _consumer;

    public KafkaConsumer(
        IServiceProvider serviceProvider,
        IEmailService emailService,
        IConfiguration configuration
        )
    {
        _serviceProvider = serviceProvider;
        _emailService = emailService;
        var kafkaConfig = new ConsumerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"],
            GroupId = configuration["Kafka:GroupId"],
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _consumer = new ConsumerBuilder<string, string>(kafkaConfig).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Starting Kafka Consumer...");
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
                    if (payload.Type.ToString() == "Applied")
                        await HandleResetPassword(payload);
                    
                }
            }
        }
        
    }

    private async Task HandleResetPassword(dynamic payload)
    {
        var recipient = payload.Email?.ToString();
        var otp = payload.OTP?.ToString();
        var firstname = payload.Firstname?.ToString();
        var notification = ResetPassword.Create(
            recipient,
            "JobTracker Password Reset Request",
            firstname,
            otp
        );

        await _emailService.Send(notification);
    }

    private async Task HandleApplied(dynamic payload)
    {
        var recipient = payload.Email?.ToString();
        var firstname = payload.Firstname?.ToString();
        var title = payload.Title?.ToString();
        var companyName = payload.CompanyName?.ToString();
        var notification = Applied.Create(
            recipient,
            $"Application Received for {title} at {companyName}",
            firstname,
            title,
            companyName
        );

        await _emailService.Send(notification);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.Close();
        await base.StopAsync(cancellationToken);
    }
}
