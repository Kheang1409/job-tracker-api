using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Queries.GetPosts;

public record GetPostsQueryWithId(string? Title, string? CompanyName, string? AuthorId, int PageNumber=1, int Limit=10) : IRequest<IEnumerable<Post>>;