using JobTracker.JobService.Domain.Enums;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdateStatusJobPost;

public record UpdateStatusJobPostWithIdCommand(
    string UserId,
    string JobPostId,
    string Status
) : IRequest<bool>;