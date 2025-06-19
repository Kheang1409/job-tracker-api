using JobTracker.JobService.Application.CustomExceptions;
using JobTracker.JobService.Application.Repositories;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.DeleteJobPost;
public class DeleteJobPostCommandHandler : IRequestHandler<DeleteJobPostCommand, bool>
{
    private readonly IJobPostRepository _jobPostRepository;
    public DeleteJobPostCommandHandler(
        IJobPostRepository jobPostRepository
    )
    {
        _jobPostRepository = jobPostRepository;
    }
    
    public async Task<bool> Handle(DeleteJobPostCommand command, CancellationToken cancellationToken)
    {
        var jobPosting = await _jobPostRepository.GetByIdAsync(command.JobPostId);
        if (jobPosting is null)
            throw new NotFoundException("Job posting not found");
        if(jobPosting.AutherId != command.AuthorId)
            throw new UnauthorizedAccessException();
        return await _jobPostRepository.DeleteAsync(command.JobPostId);
    }
}