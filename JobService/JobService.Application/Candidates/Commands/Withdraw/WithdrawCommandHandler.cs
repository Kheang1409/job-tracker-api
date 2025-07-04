using JobTracker.JobService.Application.Repositories;
using MediatR;

namespace JobTracker.JobService.Application.Candidates.Withdraw.Commands;

public class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, bool>
{
    private readonly IJobPostRepository _jobPostRepository;
    private readonly ICandidateRepository _candidateRepository;

    public WithdrawCommandHandler(
        IJobPostRepository jobPostRepository,
        ICandidateRepository candidateRepository
    )
    {
        _jobPostRepository = jobPostRepository;
        _candidateRepository = candidateRepository;
    }
    
    public async Task<bool> Handle(WithdrawCommand command, CancellationToken cancellationToken)
    {
        var job = await _jobPostRepository.GetByIdAsync(command.JobPostId);
        var candidate = job.Candidates.Single(
            candidate => candidate.Status == Domain.Enums.ApplicationStatus.Applied && candidate.CandidateId == command.CandidateId);
        candidate.Withdraw();
        return await _candidateRepository.UpdateAsync(command.JobPostId, command.CandidateId, candidate);
    }
}