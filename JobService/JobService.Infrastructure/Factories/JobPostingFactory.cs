using JobTracker.JobService.Domain.Entities;
using JobTracker.JobService.Domain.Factories;
using JobTracker.JobService.Domain.Commons;

namespace JobTracker.JobService.Infrastructure.Factories;

public class JobPostFactory : IJobPostFactory
{
    public JobPosting Create(
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
    )
    {
        var salary = SalaryRange.Create(minSalary, maxSalary, EnumParser.Currency(currency));
        var jobLocation = Location.Create(address, postalCode, city, county, state, country);
        return JobPosting.Create(
            autherId,
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
            EnumParser.Status(status));
        }
}