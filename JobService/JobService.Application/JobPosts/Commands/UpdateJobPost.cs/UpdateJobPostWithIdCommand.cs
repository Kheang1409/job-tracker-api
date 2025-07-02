using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdateJobPost;

public record UpdateJobPostWithIdCommand(
    string AuthorId,
    string JobPostId,
    string Title,
    string CompanyName,
    string WorkMode,
    string EmploymentType,
    int NumberOfOpenings,
    int MinExperience,
    int MinSalary,
    int MaxSalary,
    string Currency ,
    List<Skill> RequiredSkills,
    string JobDescription,
    string Country,
    string Street,
    string City,
    string State,
    int PostalCode,
    DateTime ExpirationDate
) : IRequest<bool>;