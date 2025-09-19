using JobTracker.JobService.Domain.Entities;

namespace JobTracker.JobService.Application.DTOs;

public class PostDetailDto
{
    public string Id { get; private set; } = string.Empty;
    public string AuthorId { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string CompanyName { get; private set; } = string.Empty;
    public string WorkMode { get; private set; } = string.Empty;
    public string EmploymentType { get; private set; } = string.Empty;
    public int NumberOfOpenings { get; private set; } = 1;
    public int MinExperience { get; private set; }
    public int MaxExperience { get; private set; }
    public decimal MinSalary { get; private set; }
    public decimal MaxSalary { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public IEnumerable<string>? Skills { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public Address? Address { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? ExpirationDate { get; private set; }


    public static explicit operator PostDetailDto(Post post)
    {
        return new PostDetailDto
        {
            Id = post.Id,
            AuthorId = post.AuthorId,
            Title = post.Title,
            CompanyName = post.CompanyName,
            WorkMode = post.WorkMode.ToString(),
            EmploymentType = post.EmploymentType.ToString(),
            NumberOfOpenings = post.NumberOfOpenings,
            MinExperience = post.MinExperience,
            MaxExperience = post.MaxExperience,
            MinSalary = post.MinSalary,
            MaxSalary = post.MaxSalary,
            Currency = post.Currency,
            Skills = post.Skills,
            Description = post.Description,
            Address = post.Address,
            Status = post.Status.ToString(),
            CreatedAt = post.CreatedAt,
            ExpirationDate = post.ExpirationDate
        };
    }
}