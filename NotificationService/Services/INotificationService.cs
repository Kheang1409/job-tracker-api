using NotificationService.Models;

public interface INotificationService
{
    Task SendResetPasswordEmail(ResetPasswordNotification resetPassword);
    Task SendReminderEmail(ReminderNotification reminder);
    Task SendGoodLuckEmail(GoodLuckNotification goodLuck);
    Task SendRejectedEmail(UpdateDateNotification rejected);
    Task SendSelectedEmail(UpdateDateNotification selected);


}
