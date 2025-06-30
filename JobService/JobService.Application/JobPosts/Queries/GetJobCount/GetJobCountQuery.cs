using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Queries.GetJobCount;

public record GetJobCountQuery(string? Title, string? CompanyName, string? AuthorId, string? CandidateId, int PageNumber=1, int Limit=10) : IRequest<int>;