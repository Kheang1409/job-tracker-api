using JobTracker.JobService.Application.Commons;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdateJobPost;

public record UpdateJobPostCommand(
    string Title,
    string CompanyName,
    string WorkMode,
    string EmploymentType,
    int NumberOfOpenings,
    int MinExperience,
    int MinSalary,
    int MaxSalary,
    string Currency ,
    List<CommandSkill> RequiredSkills,
    string JobDescription,
    string Street,
    string City,
    string State,
    string Country,
    int PostalCode,
    DateTime ExpirationDate
) : IRequest<bool>;