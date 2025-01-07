using System.ComponentModel.DataAnnotations;

namespace JobService.DTOs
{
    public class JobDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Company is required.")]
        public string Company { get; set; }
        public int? MaxPosition { get; set; } = 1;
        public int? MinExperience { get; set; } = 0;
        public double? MinSalary { get; set; } = 0.00;
        public double? MaxSalary { get; set; } = 0.00;
        public string[]? Skills { get; set; }
        public string? Description { get; set; }
        public LocationDto? Location { get; set; }
    }
}
