using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdateJobPost;

public record UpdateJobPostCommand(
    string Title,
    string CompanyName,
    string WorkMode,
    string EmploymentType,
    int NumberOfOpenings,
    int MinExperience,
    string JobDescription
) : IRequest<bool>;