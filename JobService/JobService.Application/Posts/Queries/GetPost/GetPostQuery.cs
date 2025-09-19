using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Queries.GetPost;

public record GetPostQuery(string PostId) : IRequest<Post>;