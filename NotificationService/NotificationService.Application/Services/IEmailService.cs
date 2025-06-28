using JobTracker.NotificationService.Domain.Entities;

namespace JobTracker.NotificationService.Application.Services;
public interface IEmailService
{
    Task Send<T>(T Email) where T: EmailBase;
}
