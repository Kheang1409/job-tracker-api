using JobTracker.JobService.Application.Repositories;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Queries.GetPostCount;

public class GetPostCountQueryHandler : IRequestHandler<GetPostCountQueryWithId, int>
{
    private readonly IPostRepository _jobPostRepository;

    public GetPostCountQueryHandler(
        IPostRepository jobPostRepository)
    {
        _jobPostRepository = jobPostRepository;
    }
    
    public async Task<int> Handle(GetPostCountQueryWithId command, CancellationToken cancellationToken)
    {
        var counts = await _jobPostRepository.GetPostCountAsync(
            command.Title ?? string.Empty,
            command.CompanyName ?? string.Empty,
            command.AuthorId);
        return counts;
    }
}