using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.DeletePost;


public record DeletePostCommand(string AuthorId, string JobPostId) : IRequest<bool>;