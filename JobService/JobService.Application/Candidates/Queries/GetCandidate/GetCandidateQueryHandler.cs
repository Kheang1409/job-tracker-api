using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.Candidates.Queries.GetCandidate;

public class GetCandidateQueryHandler : IRequestHandler<GetCandidateQuery, Candidate>
{
    private readonly ICandidateRepository _candidateRepository;

    public GetCandidateQueryHandler(
        ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }
    
    public async Task<Candidate> Handle(GetCandidateQuery query, CancellationToken cancellationToken)
    {
        var jobPostings = await _candidateRepository.GetByIdAsync(
            query.JobPostId,
            query.CandidateId);
        return jobPostings;
    }
}