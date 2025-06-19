using MediatR;

namespace JobTracker.JobService.Application.Candidates.Rejected.Commands;

public record RejectedCommand(
    string AuthorId,
    string JobPostId,
    string CandidateId): IRequest<bool>;