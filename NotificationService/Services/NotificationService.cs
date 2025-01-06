using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using NotificationService.Models;
using MimeKit.Text;
using Microsoft.Extensions.Options;

namespace NotificationService.Services
{
    public class NotificationService : INotificationService
    {
        private readonly EmailSettings _emailSettings;

        public NotificationService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendReminderEmail(ReminderNotification reminder)
        {
            try
            {
                var emailBody = $"Dear {reminder.Username},\n\n" +
                                $"We are pleased to inform you that {reminder.Message}" +
                                $"\n\n" +
                                $"Please make sure to be prepared and arrive on time. If you have any questions or require further assistance, don't hesitate to reach out to us.\n\n" +
                                "Best regards,\nJobTrackerApp Team";

                var message = CreateEmailMessage(reminder.Email, "Get Ready with Your Interview!", emailBody);


                await SendEmailAsync(message);
                Console.WriteLine($"Reminder email sent to {reminder.Email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send reminder email: {ex.Message}");
            }
        }

        public async Task SendGoodLuckEmail(GoodLuckNotification goodLuck)
        {
            try
            {
                var emailBody = $"Dear {goodLuck.Username},\n\n" +
                                $"{goodLuck.Message}" +
                                $"\n\n" +
                                $"We hope you have a great experience. Please make sure to be prepared and arrive on time. If you have any questions or require further assistance, don't hesitate to reach out to us.\n\n" +
                                "Best regards,\nJobTrackerApp Team";

                var message = CreateEmailMessage(goodLuck.Email, "Good Luck with Your Interview!", emailBody);

                await SendEmailAsync(message);
                Console.WriteLine($"Good luck email sent to {goodLuck.Email}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send good luck email: {ex.Message}");
            }
        }

        public async Task SendResetPasswordEmail(ResetPasswordNotification resetPassword)
        {
            try
            {
                var message = CreateEmailMessage(
                    resetPassword.Email,
                    "Reset Your Password",
                    $"Use this OPT reset your password: {resetPassword.OTP}"
                );

                await SendEmailAsync(message);
                Console.WriteLine($"Reset password email sent to {resetPassword.Email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send reset password email: {ex.Message}");
            }
        }

        public async Task SendRejectedEmail(UpdateDateNotification rejected)
        {
            try
            {
                var emailBody = $"Dear {rejected.Username},\n\n" +
                    $"{rejected.Message}" +
                    $"\n\n" +
                    $"We appreciate the time and effort you put into your application and wish you the best of luck in your future endeavors." +
                    $"\n\n" +
                    $"If you have any questions or need any feedback, feel free to reach out. We encourage you to apply for future opportunities that may align with your skills.\n\n" +
                    "Best regards,\nJobTrackerApp Team";

                var message = CreateEmailMessage(rejected.Email, "Application Update: Outcome of Your Interview", emailBody);

                await SendEmailAsync(message);
                Console.WriteLine($"Good luck email sent to {rejected.Email}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send good luck email: {ex.Message}");
            }
        }

        public async Task SendSelectedEmail(UpdateDateNotification selected)
        {
            try
            {
                var emailBody = $"Dear {selected.Username},\n\n" +
                    $"{selected.Message}" +
                    $"\n\n" +
                    $"We were impressed by your qualifications and are thrilled to welcome you to the team. Your start date is scheduled for {selected.ScheduledDate:MMMM dd, yyyy}." +
                    $"\n\n" +
                    $"We look forward to your contributions and are confident that you will make a positive impact. You will receive additional details about your first day and onboarding soon." +
                    $"\n\n" +
                    "If you have any questions or need assistance before then, feel free to reach out.\n\n" +
                    "Best regards,\nJobTrackerApp Team";

                var message = CreateEmailMessage(selected.Email, "Application Update: Outcome of Your Interview", emailBody);

                await SendEmailAsync(message);
                Console.WriteLine($"Good luck email sent to {selected.Email}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send good luck email: {ex.Message}");
            }
        }


        private MimeMessage CreateEmailMessage(string recipientEmail, string subject, string bodyText)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("JobTrackerApp", _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress(recipientEmail, recipientEmail));
            message.Subject = subject;

            message.Body = new TextPart(TextFormat.Plain) { Text = bodyText };

            return message;
        }

        private async Task SendEmailAsync(MimeMessage message)
        {
            try
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
            }
        }
    }
}