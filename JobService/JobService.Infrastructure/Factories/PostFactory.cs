using JobTracker.JobService.Domain.Entities;
using JobTracker.JobService.Domain.Factories;
using JobTracker.JobService.Domain.Enums;

namespace JobTracker.JobService.Infrastructure.Factories;

public class PostFactory : IPostFactory
{
    public Post Create(
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
    )
    {
        return new Post.Builder()
                .SetAuthorId(authorId)
                .SetTitle(title)
                .SetCompanyName(companyName)
                .SetWorkMode(workMode)
                .SetEmploymentType(employmentType)
                .SetNumberOfOpenings(numberOfOpenings)
                .SetMinExperience(minExperience)
                .SetMaxExperience(maxExperience)
                .SetMinSalary(minSalary)
                .SetMaxSalary(maxSalary)
                .SetCurrency(currency)
                .SetSkills(skills)
                .SetDescription(description)
                .SetAddress(address)
                .SetExpirationDate(expirationDate)
                .Build();
        }
}