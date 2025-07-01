using JobTracker.JobService.Application.JobLocations.Commands.CreateJobPost;
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
    List<CreateSkill> RequiredSkills,
    string JobDescription,
    string Street,
    string City,
    string State,
    string Country,
    int PostalCode
) : IRequest<bool>;