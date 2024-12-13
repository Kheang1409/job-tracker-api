using NotificationService.Models;

public interface INotificationService
{
    Task SendReminderEmail(ReminderNotification reminder);
    Task SendStatusUpdateEmail(StatusUpdateNotification statusUpdate);
    Task SendResetPasswordEmail(string email, string resetLink);
}
