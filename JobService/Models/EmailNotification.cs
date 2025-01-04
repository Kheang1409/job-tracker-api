namespace JobService.Models
{
    public class EmailNotification
    {
        public string Id { get; set; }
        public string JobId { get; set; }
        public string Email { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string? Type { get; set; }
        public string? Message { get; set; }
        public bool IsSent { get; set; } = false;
    }

}