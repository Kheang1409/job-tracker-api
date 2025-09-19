using JobTracker.JobService.Domain.Entities;
using JobTracker.JobService.Domain.Enums;

namespace JobTracker.JobService.Domain.Factories;

public interface IPostFactory
{
    Post Create(
        string authorId,
        string title,
        string companyName,
        WorkMode workMode,
        EmploymentType employmentType,
        int numberOfOpenings,
        int minExperience,
        int maxExperience,
        decimal minSalary,
        decimal maxSalary,
        string currency,
        List<string> skills,
        string description,
        Address address,
        DateTime? expirationDate
    );
}