using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Queries.GetPosts;

public class GetPostsQueryHandler : IRequestHandler<GetPostsQueryWithId, IEnumerable<Post>>
{
    private readonly IPostRepository _jobPostRepository;

    public GetPostsQueryHandler(
        IPostRepository jobPostRepository)
    {
        _jobPostRepository = jobPostRepository;
    }
    
    public async Task<IEnumerable<Post>> Handle(GetPostsQueryWithId command, CancellationToken cancellationToken)
    {
        var jobPostings = await _jobPostRepository.GetAllAsync(
            command.Title ?? string.Empty,
            command.CompanyName ?? string.Empty,
            command.AuthorId ?? string.Empty,
            command.PageNumber,
            command.Limit);
        return jobPostings;
    }
}