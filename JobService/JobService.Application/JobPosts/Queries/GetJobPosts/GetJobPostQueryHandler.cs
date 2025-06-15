using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Queries.GetJobPosts;

public class GetJobPostsQueryHandler : IRequestHandler<GetJobPostsQuery, IEnumerable<JobPosting>>
{
    private readonly IJobPostRepository _jobPostRepository;

    public GetJobPostsQueryHandler(
        IJobPostRepository jobPostRepository)
    {
        _jobPostRepository = jobPostRepository;
    }
    
    public async Task<IEnumerable<JobPosting>> Handle(GetJobPostsQuery command, CancellationToken cancellationToken)
    {
        var jobPostings = await _jobPostRepository.GetAllAsync(
            command.Title ?? string.Empty,
            command.CompanyName ?? string.Empty,
            command.PageNumber,
            command.Limit);
        return jobPostings;
    }
}