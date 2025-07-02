using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Commons;
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
        if(updateJobPosting.AuthorId != command.AuthorId)
            throw new UnauthorizedAccessException();
        updateJobPosting.Update(
            command.Title,
            command.CompanyName,
            command.WorkMode,
            command.EmploymentType,
            command.NumberOfOpenings,
            command.MinExperience,
            command.MinSalary,
            command.MaxSalary,
            command.Currency,
            command.RequiredSkills,
            command.JobDescription,
            command.Street,
            command.City,
            command.State,
            command.Country,
            command.PostalCode,
            command.ExpirationDate
        );
        return await _jobPostRepository.UpdateAsync(updateJobPosting);
    }
}