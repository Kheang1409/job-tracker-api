using JobTracker.JobService.Domain.Enums;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdatePostStatus;

public record UpdatePostStatusWithIdCommand(
    string UserId,
    string JobPostId,
    Status Status
) : IRequest<bool>;