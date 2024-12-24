namespace NotificationService.Models
{
    public class ResetPasswordNotification : NotificationBase
    {
        public string Message { get; set; }
        public string ResetLink { get; set; }
    }
}