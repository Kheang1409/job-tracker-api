namespace NotificationService.Models;

public class StatusUpdateNotification : NotificationBase
{
    public string Email { get; set; }
    public string JobTitle { get; set; }
    public string Status { get; set; }
}
