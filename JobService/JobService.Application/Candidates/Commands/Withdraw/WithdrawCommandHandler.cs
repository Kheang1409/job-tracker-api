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
        var candidate = await _candidateRepository.GetByIdAsync(command.JobPostId, command.CandidateId);
        candidate.Withdraw();
        return await _candidateRepository.UpdateAsync(command.JobPostId, command.CandidateId, candidate);
    }
}