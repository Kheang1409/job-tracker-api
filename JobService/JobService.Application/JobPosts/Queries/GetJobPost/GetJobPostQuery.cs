using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Queries.GetJobPost;

public record GetJobPostQuery(string JobPostId) : IRequest<JobPosting>;