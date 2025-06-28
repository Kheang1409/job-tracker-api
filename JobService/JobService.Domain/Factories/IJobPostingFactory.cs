using JobTracker.JobService.Domain.Entities;

namespace JobTracker.JobService.Domain.Factories;

public interface IJobPostFactory
{
    JobPosting Create(
        string autherId,
        string title,
        string companyName,
        string workMode,
        string employmentType,
        int numberOfOpenings,
        int minExperience,
        int minSalary,
        int maxSalary,
        string currency,
        List<Skill> requiredSkills,
        string jobDescription,
        string address,
        int postalCode,
        string city,
        string county,
        string state,
        string country,
        string status
    );
}