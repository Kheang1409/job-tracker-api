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
    string JobDescription) : IRequest<bool>;