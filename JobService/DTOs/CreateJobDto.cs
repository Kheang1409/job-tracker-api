namespace JobService.DTOs
{
    public class CreateJobDto
    {
        public string Title { get; set; }
        public string Company { get; set; }
        public DateTime? AppliedDate { get; set; }

    }
}
