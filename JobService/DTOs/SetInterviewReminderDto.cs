namespace JobService.DTOs
{
    public class SetInterviewReminderDto
    {
        public DateTime? InterviewDate { get; set; }
        public int ReminderDaysBeforeInterview { get; set; }
    }
}
