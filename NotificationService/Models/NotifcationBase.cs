namespace NotificationService.Models;

public abstract class NotificationBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Type { get; set; }
    public string Message { get; set; }
}