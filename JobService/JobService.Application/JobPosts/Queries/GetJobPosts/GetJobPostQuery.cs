using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Queries.GetJobPosts;

public record GetJobPostsQuery(string? Title, string? CompanyName, string? Skill, int PageNumber=1, int Limit=10) : IRequest<IEnumerable<JobPosting>>;