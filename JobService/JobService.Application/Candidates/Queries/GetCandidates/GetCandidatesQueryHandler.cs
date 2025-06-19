using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.Candidates.Queries.GetCandidates;

public class GetCandidatesQueryHandler : IRequestHandler<GetCandidatesWithIdQuery, IEnumerable<Candidate>>
{
    private readonly ICandidateRepository _candidateRepository;

    public GetCandidatesQueryHandler(
        ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }
    
    public async Task<IEnumerable<Candidate>> Handle(GetCandidatesWithIdQuery query, CancellationToken cancellationToken)
    {
        var jobPostings = await _candidateRepository.GetAllAsync(
            query.JobPostId,
            query.PageNumber,
            query.Limit);
        return jobPostings;
    }
}