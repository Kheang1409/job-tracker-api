using JobTracker.JobService.Application.Repositories;
using JobTracker.SharedKernel.Exceptions;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.DeletePost;
public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, bool>
{
    private readonly IPostRepository _jobPostRepository;
    public DeletePostCommandHandler(
        IPostRepository jobPostRepository
    )
    {
        _jobPostRepository = jobPostRepository;
    }
    
    public async Task<bool> Handle(DeletePostCommand command, CancellationToken cancellationToken)
    {
        var jobPosting = await _jobPostRepository.GetByIdAsync(command.JobPostId);
        if (jobPosting is null)
            throw new NotFoundException("Job posting not found");
        if(jobPosting.AuthorId != command.AuthorId)
            throw new UnauthorizedAccessException();
        return await _jobPostRepository.DeleteAsync(command.JobPostId);
    }
}