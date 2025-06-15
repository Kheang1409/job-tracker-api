using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Commons;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdateStatusJobPost;

public class UpdateStatusJobPostCommandHandler : IRequestHandler<UpdateStatusJobPostWithIdCommand, bool>
{
    private readonly IJobPostRepository _jobPostRepository;

    public UpdateStatusJobPostCommandHandler(
        IJobPostRepository jobPostRepository
    )
    {
        _jobPostRepository = jobPostRepository;
    }
    
    
    public async Task<bool> Handle(UpdateStatusJobPostWithIdCommand command, CancellationToken cancellationToken)
    {
        var jobPosting = await _jobPostRepository.GetByIdAsync(command.JobPostId);
        jobPosting.UpdateStatus(EnumParser.Status(command.Status));
        if (jobPosting.AutherId != command.UserId)
            throw new UnauthorizedAccessException();
        return await _jobPostRepository.UpdateAsync(jobPosting);
    }
}