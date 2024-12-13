using Confluent.Kafka;
using Newtonsoft.Json;
using NotificationService.Models;
using NotificationService.Services;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;

namespace NotificationService.KafkaConsumers;

public class NotificationConsumer : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly INotificationService _notificationService;

    public NotificationConsumer(IConsumer<string, string> consumer, INotificationService notificationService)
    {
        _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Starting Kafka consumer for 'job-topic'...");

        _consumer.Subscribe("job-topic");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var message = _consumer.Consume(stoppingToken);

                if (message != null && !string.IsNullOrEmpty(message.Value))
                {
                    Console.WriteLine($"Received message from Kafka: {message.Value}");

                    try
                    {
                        var payload = JsonConvert.DeserializeObject<dynamic>(message.Value);

                        // Validate the message type and required fields
                        if (payload?.Type == "resetPassword" && IsValidEmail(payload?.Email?.ToString()))
                        {
                            var email = payload?.Email.ToString();
                            var resetLink = payload?.ResetLink.ToString();

                            await SendEmailNotification(email, resetLink);
                        }
                        else
                        {
                            Console.WriteLine("Invalid message payload or missing fields. Skipping...");
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Failed to deserialize message: {ex.Message}. Skipping...");
                    }
                }
                else
                {
                    Console.WriteLine("Received empty or null message. Skipping...");
                }
            }
            catch (ConsumeException ex)
            {
                Console.WriteLine($"Kafka Consumer Error: {ex.Error.Reason}");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Retry with delay
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Consumer operation canceled. Shutting down...");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error in Kafka consumer: {ex.Message}");
            }
        }

        Console.WriteLine("Kafka consumer stopped.");
    }

    private async Task SendEmailNotification(string email, string resetLink)
    {
        var reminder = new ReminderNotification
        {
            Email = email,
            Message = "Please reset your password.",
            ResetLink = resetLink
        };

        try
        {
            Console.WriteLine($"Sending email notification to {email}...");
            await _notificationService.SendReminderEmail(reminder);
            Console.WriteLine($"Email notification successfully sent to {email}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Email Notification Failed: {e.Message}");
        }
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        try
        {
            // Simple regex for email validation
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }
        catch
        {
            return false;
        }
    }

    public override void Dispose()
    {
        Console.WriteLine("Disposing Kafka consumer...");
        _consumer?.Close(); // Commit offsets and close cleanly
        _consumer?.Dispose();
        base.Dispose();
    }
}