using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.DeleteJobPost;


public record DeleteJobPostCommand(string AuthorId, string JobPostId) : IRequest<bool>;