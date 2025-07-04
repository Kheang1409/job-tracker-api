using JobTracker.JobService.Application.Commons;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.CreateJobPost;

public record CreateJobPostWithIdCommand(
    string AuthorId,
    string Title,
    string CompanyName,
    string WorkMode,
    string EmploymentType,
    int NumberOfOpenings,
    int MinExperience,
    int MinSalary,
    int MaxSalary,
    string Currency,
    List<CommandSkill> RequiredSkills,
    string JobDescription,
    string Country,
    string Street,
    string City,
    string State,
    int PostalCode,
    DateTime ExpirationDate
): IRequest<string>;