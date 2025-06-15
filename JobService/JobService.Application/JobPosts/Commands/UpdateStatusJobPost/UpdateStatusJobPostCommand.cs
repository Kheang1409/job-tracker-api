using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdateStatusJobPost;

public record UpdateStatusJobPostCommand(
    string Status
) : IRequest<bool>;