using Microsoft.Extensions.DependencyInjection;
using JobTracker.NotificationService.Application.Services;
using JobTracker.NotificationService.Infrastructure.Services;
using JobTracker.NotificationService.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace JobTracker.NotificationService.Infrastructure.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var emailSettingsSection = configuration.GetSection("EmailSettings");
        
        string smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER")
                            ?? emailSettingsSection["SmtpServer"]
                            ?? throw new ArgumentException("Email setting 'SmtpServer' is missing or empty.");
        string portString = Environment.GetEnvironmentVariable("SMTP_PORT")
                            ?? emailSettingsSection["Port"]
                            ?? throw new ArgumentException("Email setting 'Port' is missing or not a valid integer.");
        string senderEmail = Environment.GetEnvironmentVariable("SMTP_SENDER_EMAIL")
                            ?? emailSettingsSection["SenderEmail"]
                            ?? throw new ArgumentException("Email setting 'SenderEmail' is missing or empty.");
        string senderPassword = Environment.GetEnvironmentVariable("SMTP_SENDER_PASSWORD")
                            ?? emailSettingsSection["SenderPassword"]
                            ?? throw new ArgumentException("Email setting 'SenderPassword' is missing or empty.");

        var emailSettings = new EmailSettings(smtpServer, int.Parse(portString), senderEmail, senderPassword);
        services.AddSingleton(emailSettings);
        services.AddSingleton<IEmailService, EmailService>();
        return services;
    }
}
