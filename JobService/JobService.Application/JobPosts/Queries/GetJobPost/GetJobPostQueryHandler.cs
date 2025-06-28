using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Queries.GetJobPost;

public class GetJobPostQueryHandler : IRequestHandler<GetJobPostQuery, JobPosting>
{
    private readonly IJobPostRepository _jobPostRepository;

    public GetJobPostQueryHandler(
        IJobPostRepository jobPostRepository)
    {
        _jobPostRepository = jobPostRepository;
    }
    
    public async Task<JobPosting> Handle(GetJobPostQuery command, CancellationToken cancellationToken)
    {
        var jobPosting = await _jobPostRepository.GetByIdAsync(command.JobPostId);
        if(jobPosting is null)
            throw new InvalidOperationException($"The job posting is not exits.");
        return jobPosting;
    }
}