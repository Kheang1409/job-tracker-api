using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Queries.GetPostCount;

public record GetPostCountQuery(string? Title, string? CompanyName, int PageNumber=1, int Limit=10) : IRequest<int>;