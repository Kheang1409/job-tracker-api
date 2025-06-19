using MediatR;

namespace JobTracker.JobService.Application.Candidates.MoveOn.Commands;

public record MoveOnWithIdCommand(
    string AuthorId,
    string JobPostId,
    string CandidateId,
    string Name,
    DateTime AppointmentDate): IRequest<bool>;