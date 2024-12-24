namespace NotificationService.Models;

public class StatusUpdateNotification : NotificationBase
{
    public string JobTitle { get; set; }
    public string Status { get; set; }
}
