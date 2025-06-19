using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit.Text;
using JobTracker.NotificationService.Application.Services;
using JobTracker.NotificationService.Domain.Entities;

namespace JobTracker.NotificationService.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(EmailSettings emailSettings)
    {
        _emailSettings = emailSettings;
    }

    public async Task Send<T>(T Email) where T : EmailBase
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("JobTrackerApp", _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress(Email.Recipient, Email.Recipient));
            message.Subject = Email.Subject;
            message.Body = new TextPart(TextFormat.Html) { Text = Email.Message() };
            await SendEmailAsync(message);
        }
        catch (Exception)
        {
            throw;
        }
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
        }
        catch (Exception)
        {
            throw;
        }
    }
}