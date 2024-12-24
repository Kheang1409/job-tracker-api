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
        private readonly IConfiguration _configuration;
        private readonly EmailSettings _emailSettings;

        public NotificationService(IConfiguration configuration, IOptions<EmailSettings> emailSettings)
        {
            _configuration = configuration;
            _emailSettings = emailSettings.Value;
        }

        public async Task SendReminderEmail(ReminderNotification reminder)
        {
            try
            {
                // Construct the email body with the reminder message and optional scheduled date
                var emailBody = reminder.ScheduledDate != null
                    ? $"Reminder: {reminder.Message}\n\nScheduled Date: {reminder.ScheduledDate:MMMM dd, yyyy HH:mm}"
                    : $"Reminder: {reminder.Message}";

                var message = CreateEmailMessage(reminder.Email, "JobTrackerApp - Reminder Notification", emailBody);

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
                // Construct the email body
                var emailBody = $"Hello,\n\nGood luck with your interview! " +
                                $"We hope you have a great experience. If you have any questions or need support, feel free to reach out.\n\n" +
                                "Best regards,\nJobTrackerApp Team";

                // Create the email message
                var message = CreateEmailMessage(goodLuck.Email, "Good Luck with Your Interview!", emailBody);

                // Send the email
                await SendEmailAsync(message);
                Console.WriteLine($"Good luck email sent to {goodLuck.Email}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send good luck email: {ex.Message}");
            }
        }

        public async Task SendStatusUpdateEmail(StatusUpdateNotification statusUpdate)
        {
            try
            {
                var message = CreateEmailMessage(
                    statusUpdate.Email,
                    "Job Application Status Update",
                    $"Dear {statusUpdate.Email},\n\nYour job application for '{statusUpdate.JobTitle}' has been updated to status: {statusUpdate.Status}.\n\nBest regards,\nJob Tracker"
                );

                await SendEmailAsync(message);
                Console.WriteLine($"Status update email sent to {statusUpdate.Email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send status update email: {ex.Message}");
            }
        }

        public async Task SendResetPasswordEmail(ResetPasswordNotification resetPassword)
        {
            try
            {
                var message = CreateEmailMessage(
                    resetPassword.Email,
                    "Reset Your Password",
                    $"Click this link to reset your password: {resetPassword.ResetLink}"
                );

                await SendEmailAsync(message);
                Console.WriteLine($"Reset password email sent to {resetPassword.Email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send reset password email: {ex.Message}");
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