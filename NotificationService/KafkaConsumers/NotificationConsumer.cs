using Confluent.Kafka;
using Hangfire;
using Newtonsoft.Json;
using NotificationService.Models;
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

                        // Check the notification type and route to appropriate handler
                        string type = payload?.Type?.ToString();
                        switch (type)
                        {
                            case "resetPassword":
                                await HandleResetPassword(payload);
                                break;

                            case "statusUpdate":
                                await HandleStatusUpdate(payload);
                                break;

                            case "reminder":
                                await HandleReminder(payload);
                                break;

                            case "goodLuck":
                                await HandleGoodLuck(payload);
                                break;

                            default:
                                Console.WriteLine($"Unknown notification type: {type}. Skipping...");
                                break;
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

    // Handler for Reset Password Notification (Existing Logic)
    private async Task HandleResetPassword(dynamic payload)
    {
        if (payload?.Email == null || payload?.ResetLink == null || !IsValidEmail(payload?.Email.ToString()))
        {
            Console.WriteLine("Invalid ResetPassword payload. Skipping...");
            return;
        }

        string email = payload.Email.ToString();
        string resetLink = payload.ResetLink.ToString();

        var notification = new ResetPasswordNotification
        {
            Type = "resetPassword",
            Email = email,
            Message = "Please reset your password.",
            ResetLink = resetLink
        };

        await SendNotification(notification, async () =>
            await _notificationService.SendResetPasswordEmail(notification));
    }

    // Handler for Status Update Notification
    private async Task HandleStatusUpdate(dynamic payload)
    {
        if (payload?.Email == null || payload?.JobTitle == null || payload?.Status == null ||
            !IsValidEmail(payload?.Email.ToString()))
        {
            Console.WriteLine("Invalid StatusUpdate payload. Skipping...");
            return;
        }

        var notification = new StatusUpdateNotification
        {
            Type = "statusUpdate",
            Email = payload.Email.ToString(),
            JobTitle = payload.JobTitle.ToString(),
            Status = payload.Status.ToString()
        };

        await SendNotification(notification, async () =>
            await _notificationService.SendStatusUpdateEmail(notification));
    }

    // Handler for Reminder Notification
    private async Task HandleReminder(dynamic payload)
    {
        if (payload?.Email == null || payload?.Message == null || payload?.ScheduledDate == null ||
            !IsValidEmail(payload?.Email.ToString()))
        {
            Console.WriteLine($"Check: {payload?.Email}, {payload?.Message}, {payload?.ScheduledDate}");
            Console.WriteLine("Invalid Reminder payload. Skipping...");
            return;
        }

        var notification = new ReminderNotification
        {
            Type = "reminder",
            Email = payload.Email.ToString(),
            Message = payload.Message.ToString(),
            ScheduledDate = DateTime.Parse(payload.ScheduledDate.ToString())
        };

        var timeUntilSend = notification.ScheduledDate - DateTime.UtcNow;

        if (timeUntilSend <= TimeSpan.Zero)
        {
            await SendNotification(notification, async () =>
                await _notificationService.SendReminderEmail(notification));
        }
        else
        {
            BackgroundJob.Schedule(() =>
                _notificationService.SendReminderEmail(notification), timeUntilSend);
        }
    }

    // Handler for Good Luck Notification
    private async Task HandleGoodLuck(dynamic payload)
    {
        if (payload?.Email == null || payload?.Message == null || !IsValidEmail(payload?.Email.ToString()))
        {
            Console.WriteLine($"Check: {payload?.Email}, {payload?.Message}, {payload?.ScheduledDate}");
            Console.WriteLine("Invalid GoodLuck payload. Skipping...");
            return;
        }

        var notification = new GoodLuckNotification
        {
            Type = "goodLuck",
            Email = payload.Email.ToString(),
            Message = payload.Message.ToString(),
            ScheduledDate = DateTime.Parse(payload.ScheduledDate.ToString())
        };

        var timeUntilSend = notification.ScheduledDate - DateTime.UtcNow;

        if (timeUntilSend <= TimeSpan.Zero)
        {
            await SendNotification(notification, async () =>
                await _notificationService.SendGoodLuckEmail(notification));
        }
        else
        {
            BackgroundJob.Schedule(() =>
                _notificationService.SendGoodLuckEmail(notification), timeUntilSend);
        }
    }

    // Helper to validate email format
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

    // Helper to send notifications and handle errors
    private async Task SendNotification<T>(T notification, Func<Task> sendEmailFunc) where T : NotificationBase
    {
        try
        {
            Console.WriteLine($"Sending {notification.Type} notification to {notification.Email}...");
            await sendEmailFunc();
            Console.WriteLine($"{notification.Type} notification successfully sent to {notification.Email}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send {notification.Type} notification: {ex.Message}");
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