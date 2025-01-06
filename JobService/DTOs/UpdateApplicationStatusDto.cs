namespace JobService.DTOs
{
    public class UpdateApplicationStatusDto
    {
        public string? Notes { get; set; }
        public string Status { get; set; }

        public DateTime? StartDate { get; set; }
    }
}
