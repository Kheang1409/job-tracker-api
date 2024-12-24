using NotificationService.Models;

public interface INotificationService
{
    Task SendStatusUpdateEmail(StatusUpdateNotification statusUpdate);
    Task SendResetPasswordEmail(ResetPasswordNotification resetPassword);
    Task SendReminderEmail(ReminderNotification reminder);
    Task SendGoodLuckEmail(GoodLuckNotification goodLuck);
}
