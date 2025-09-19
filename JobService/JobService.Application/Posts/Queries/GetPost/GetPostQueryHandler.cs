using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Queries.GetPost;

public class GetPostQueryHandler : IRequestHandler<GetPostQuery, Post>
{
    private readonly IPostRepository _jobPostRepository;

    public GetPostQueryHandler(
        IPostRepository jobPostRepository)
    {
        _jobPostRepository = jobPostRepository;
    }
    
    public async Task<Post> Handle(GetPostQuery command, CancellationToken cancellationToken)
    {
        var jobPosting = await _jobPostRepository.GetByIdAsync(command.PostId);
        if(jobPosting is null)
            throw new InvalidOperationException($"The job posting is not exits.");
        return jobPosting;
    }
}