using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Queries.GetPostCount;

public record GetPostCountQueryWithId(string? Title, string? CompanyName, string AuthorId, int PageNumber=1, int Limit=10) : IRequest<int>;