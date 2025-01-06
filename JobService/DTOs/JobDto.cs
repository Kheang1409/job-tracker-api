namespace JobService.DTOs
{
    public class JobDto
    {
        public string Title { get; set; }
        public string Company { get; set; }
        public int? MinExperience { get; set; } = 0;
        public int? MaxExperience { get; set; } = 0;
        public double? MinSalary { get; set; } = 0.00;
        public double? MaxSalary { get; set; } = 0.00;
        public string[]? Skills { get; set; }
        public string? Description { get; set; }
        public LocationDto? Location { get; set; }
    }
}
