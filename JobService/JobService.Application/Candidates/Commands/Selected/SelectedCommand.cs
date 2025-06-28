using MediatR;

namespace JobTracker.JobService.Application.Candidates.Selected.Commands;

public record SelectedCommand(
    string AuthorId,
    string JobPostId,
    string CandidateId): IRequest<bool>;