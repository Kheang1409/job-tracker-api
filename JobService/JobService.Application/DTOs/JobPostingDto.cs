using JobTracker.JobService.Domain.Entities;

namespace JobTracker.JobService.Application.DTOs;

public class JobPostingDto
{
    public string Id { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string CompanyName { get; private set; } = string.Empty;
    public string WorkMode { get; private set; } = string.Empty;
    public string EmploymentType { get; private set; } = string.Empty;
    public SalaryRangeDto? Salary { get; private set; }
    public Location? JobLocation { get; private set; }
    public int NumberOfOpenings { get; private set; } = 1;
    public string Status { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }


    public static explicit operator JobPostingDto(JobPosting jobPosting)
    {
        return new JobPostingDto
        {
            Id = jobPosting.Id,
            Title = jobPosting.Title,
            CompanyName = jobPosting.CompanyName,
            WorkMode = jobPosting.WorkMode.ToString(),
            EmploymentType = jobPosting.EmploymentType.ToString(),
            Salary = jobPosting.SalaryRange is not null ? (SalaryRangeDto)jobPosting.SalaryRange : null,
            JobLocation = jobPosting.JobLocation,
            NumberOfOpenings = jobPosting.NumberOfOpenings,
            Status = jobPosting.Status.ToString(),
            CreatedAt = jobPosting.CreatedAt
        };
    }
}