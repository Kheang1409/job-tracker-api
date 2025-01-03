using JobService.Models;

namespace JobService.DTOs
{
    public class CreateJobDto
    {
        public string Title { get; set; }
        public string Company { get; set; }
        public string[]? Skills { get; set; }
        public string? Description { get; set; }
        public Location? Location { get; set; }
        public string Status { get; set; }
        public DateTime? AppliedDate { get; set; }

    }
}
