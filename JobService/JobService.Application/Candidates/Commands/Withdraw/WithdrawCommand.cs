using MediatR;

namespace JobTracker.JobService.Application.Candidates.Withdraw.Commands;

public record WithdrawCommand(
    string JobPostId,
    string CandidateId): IRequest<bool>;