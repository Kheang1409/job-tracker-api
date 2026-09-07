using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace JobTracker.EmailService;

public sealed class EmailSender(IConfiguration configuration, EmailTemplateRenderer templateRenderer)
{
    private readonly string _smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER") ?? configuration["EmailSettings:SmtpServer"] ?? throw new ArgumentException("SMTP_SERVER is required.");
    private readonly int _port = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? configuration["EmailSettings:Port"], out var port) ? port : throw new ArgumentException("SMTP_PORT is invalid.");
    private readonly string _senderEmail = Environment.GetEnvironmentVariable("SMTP_SENDER_EMAIL") ?? configuration["EmailSettings:SenderEmail"] ?? throw new ArgumentException("SMTP_SENDER_EMAIL is required.");
    private readonly string _senderPassword = NormalizePassword(
        Environment.GetEnvironmentVariable("SMTP_SENDER_PASSWORD")
        ?? configuration["EmailSettings:SenderPassword"]
        ?? throw new ArgumentException("SMTP_SENDER_PASSWORD is required."));
    private readonly string _frontendOrigin = Environment.GetEnvironmentVariable("FRONTEND_ORIGIN") ?? configuration["EmailSettings:FrontendOrigin"] ?? "http://localhost:4200";

    public async Task SendAsync(EmailRequest request, CancellationToken cancellationToken)
    {
        var (subject, body) = templateRenderer.Render(request, _frontendOrigin);
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("JobTracker", _senderEmail));
        message.To.Add(new MailboxAddress(request.FirstName, request.Recipient));
        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Html) { Text = body };

        using var client = new SmtpClient();
        var socketOptions = _port == 465
            ? SecureSocketOptions.SslOnConnect
            : SecureSocketOptions.StartTls;
        await client.ConnectAsync(_smtpServer, _port, socketOptions, cancellationToken);
        await client.AuthenticateAsync(_senderEmail, _senderPassword, cancellationToken);
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }

    private static string NormalizePassword(string password) =>
        string.Concat(password.Where(character => !char.IsWhiteSpace(character)));
}
