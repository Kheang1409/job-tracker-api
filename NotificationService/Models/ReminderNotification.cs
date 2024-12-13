namespace NotificationService.Models;

public class ReminderNotification : NotificationBase
{
    public string Email { get; set; }
    public string Message { get; set; }
    public string ResetLink { get; set; }
}
