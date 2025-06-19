using MediatR;

namespace JobTracker.JobService.Application.Candidates.MoveOn.Commands;

public record MoveOnCommand(
    string Name,
    DateTime AppointmentDate): IRequest<bool>;