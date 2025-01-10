using NotificationService.Models;

public interface INotificationService
{
    Task SendResetPasswordEmail(ResetPasswordNotification resetPassword);
    Task SendUpdateStatusEmail(UpdateStatusNotification updateStatus);
    Task SendGoodLuckEmail(GoodLuckNotification goodLuck);


}
