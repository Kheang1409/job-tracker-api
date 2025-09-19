using JobTracker.JobService.Domain.Enums;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdatePostStatus;

public record UpdateStatusPostCommand(
    Status Status
) : IRequest<bool>;