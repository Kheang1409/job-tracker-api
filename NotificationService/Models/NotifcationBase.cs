namespace NotificationService.Models;

public abstract class NotificationBase
{
    public string Email { get; set; }
    public string Type { get; set; }
    public DateTime ScheduledDate { get; set; }
}