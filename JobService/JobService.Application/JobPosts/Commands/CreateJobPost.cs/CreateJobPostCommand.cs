using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.CreateJobPost;

public record CreateJobPostCommand(
    string Title,
    string CompanyName,
    string WorkMode,
    string EmploymentType,
    int NumberOfOpenings,
    int MinExperience,
    int MinSalary,
    int MaxSalary,
    string Currency ,
    List<CreateSkill> RequiredSkills,
    string JobDescription,
    string Country,
    string Street,
    string City,
    string State,
    int PostalCode,
    DateTime ExpirationDate
) : IRequest<string>;

public record CreateSkill(
    string Name
);
