using JobService.Models;

namespace JobService.DTOs
{
    public class JobDto
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string[]? Skills { get; set; }
        public string? Description { get; set; }
        public Location? Location { get; set; }
        public DateTime? AppliedDate { get; set; }
        public string? Status { get; set; }
        public DateTime? InterviewDate { get; set; }
        public int? ReminderDaysBeforeInterview { get; set; }
    }
}
