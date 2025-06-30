using JobTracker.JobService.Application.Repositories;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Queries.GetJobCount;

public class GetJobCountQueryHandler : IRequestHandler<GetJobCountQuery, int>
{
    private readonly IJobPostRepository _jobPostRepository;

    public GetJobCountQueryHandler(
        IJobPostRepository jobPostRepository)
    {
        _jobPostRepository = jobPostRepository;
    }
    
    public async Task<int> Handle(GetJobCountQuery command, CancellationToken cancellationToken)
    {
        var counts = await _jobPostRepository.GetJobCountAsync(
            command.Title ?? string.Empty,
            command.CompanyName ?? string.Empty,
            command.AuthorId ?? string.Empty,
            command.CandidateId ?? string.Empty);
        return counts;
    }
}