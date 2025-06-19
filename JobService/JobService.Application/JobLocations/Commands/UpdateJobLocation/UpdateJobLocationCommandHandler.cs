using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdateJobLocation;

public class UpdateJobLocationCommandHandler : IRequestHandler<UpdateJobLocationWithIdCommand, bool>
{
    private readonly IJobPostRepository _jobPostRepository;
    public UpdateJobLocationCommandHandler(
        IJobPostRepository jobPostRepository
    )
    {
        _jobPostRepository = jobPostRepository;
    }
    
    public async Task<bool> Handle(UpdateJobLocationWithIdCommand command, CancellationToken cancellationToken)
    {
        var updateJobPosting = await _jobPostRepository.GetByIdAsync(command.JobPostId);
        if(updateJobPosting.AutherId != command.AuthorId)
            throw new UnauthorizedAccessException();
        updateJobPosting.JobLocation?.Update(
            command.Address,
            command.PostalCode,
            command.City,
            command.County,
            command.State,
            command.Country
        );
        return await _jobPostRepository.UpdateAsync(updateJobPosting);
    }
}