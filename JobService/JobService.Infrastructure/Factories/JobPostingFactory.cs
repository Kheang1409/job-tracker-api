using JobTracker.JobService.Domain.Entities;
using JobTracker.JobService.Domain.Factories;
using JobTracker.JobService.Domain.Commons;

namespace JobTracker.JobService.Infrastructure.Factories;

public class JobPostFactory : IJobPostFactory
{
    public JobPosting Create(
        string authorId,
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
        string street,
        string city,
        string state,
        string country,
        int postalCode,
        DateTime expirationDate
    )
    {
        var salary = SalaryRange.Create(minSalary, maxSalary, currency);
        var jobLocation = Address.Create(country, street, city, state, postalCode);
        return JobPosting.Create(
            authorId,
            title,
            companyName,
            EnumParser.WorkMode(workMode),
            EnumParser.EmploymentType(employmentType),
            numberOfOpenings,
            minExperience,
            salary,
            requiredSkills,
            jobDescription,
            jobLocation,
            expirationDate);
        }
}