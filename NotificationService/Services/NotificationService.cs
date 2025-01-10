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

        public async Task SendGoodLuckEmail(GoodLuckNotification goodLuck)
        {
            try
            {
                var emailBody = goodLuck.Message;
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

        public async Task SendUpdateStatusEmail(UpdateStatusNotification rejected)
        {
            try
            {
                var emailBody = rejected.Message;
                var message = CreateEmailMessage(rejected.Email, "Updated About You Application!", emailBody);

                await SendEmailAsync(message);
                Console.WriteLine($"Update status email sent to {rejected.Email}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send update status email: {ex.Message}");
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