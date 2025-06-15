using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.DeleteJobPost;


public record DeleteJobPostCommand(string UserId, string JobPostId) : IRequest<bool>;