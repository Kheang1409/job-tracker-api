namespace JobTracker.NotificationService.Domain.Entities;

public class EmailSettings
{
    public string SmtpServer { get; private set; } = string.Empty;
    public int Port { get; set; }
    public string SenderEmail { get; private set; } = string.Empty;
    public string SenderPassword { get; private set; } = string.Empty;

    public EmailSettings(
        string smtpServer,
        int port,
        string senderEmail,
        string senderPassword)
    {
        SmtpServer = smtpServer;
        Port = port;
        SenderEmail = senderEmail;
        SenderPassword = senderPassword;
    }

}