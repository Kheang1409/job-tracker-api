namespace JobService.DTOs
{
    public class JobDto
    {
        public string Title { get; set; }
        public string Company { get; set; }
        public DateTime? AppliedDate { get; set; }
        public string Status { get; set; }
        public DateTime? InterviewDate { get; set; }
        public int? ReminderDaysBeforeInterview { get; set; }
    }
}
