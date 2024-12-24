namespace NotificationService.Models;

public class ReminderNotification : NotificationBase
{
    public string Message { get; set; }
    public DateTime ScheduledDate { get; set; }
}
