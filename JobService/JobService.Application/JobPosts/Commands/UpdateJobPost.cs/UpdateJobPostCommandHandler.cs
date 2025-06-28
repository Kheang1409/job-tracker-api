using JobTracker.JobService.Application.Repositories;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdateJobPost;

public class UpdateJobPostCommandHandler : IRequestHandler<UpdateJobPostWithIdCommand, bool>
{
    private readonly IJobPostRepository _jobPostRepository;
    public UpdateJobPostCommandHandler(
        IJobPostRepository jobPostRepository
    )
    {
        _jobPostRepository = jobPostRepository;
    }
    
    public async Task<bool> Handle(UpdateJobPostWithIdCommand command, CancellationToken cancellationToken)
    {
        var updateJobPosting = await _jobPostRepository.GetByIdAsync(command.JobPostId);
        if(updateJobPosting.AutherId != command.AuthorId)
            throw new UnauthorizedAccessException();
        updateJobPosting.Update(
            command.Title,
            command.CompanyName,
            command.NumberOfOpenings,
            command.MinExperience,
            command.JobDescription
        );
        return await _jobPostRepository.UpdateAsync(updateJobPosting);
    }
}