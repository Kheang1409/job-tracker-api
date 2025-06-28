using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Enums;
using JobTracker.JobService.Domain.Factories;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.CreateJobPost;

public class CreateJobPostCommandHandler : IRequestHandler<CreateJobPostWithIdCommand, string>
{
    private readonly IJobPostRepository _jobPostRepository;
    private readonly IJobPostFactory _jobPostFactory;

    public CreateJobPostCommandHandler(
        IJobPostRepository jobPostRepository,
        IJobPostFactory jobPostFactory
    )
    {
        _jobPostFactory = jobPostFactory;
        _jobPostRepository = jobPostRepository;
    }
    
    public async Task<string> Handle(CreateJobPostWithIdCommand command, CancellationToken cancellationToken)
    {
        var jobPosting = _jobPostFactory.Create(
            command.AuthorId,
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
            command.Address,
            command.PostalCode,
            command.City,
            command.County,
            command.State,
            command.Country,
            command.Status
        );
        var jobPostId = await _jobPostRepository.AddAsync(jobPosting);
        return jobPostId;
    }
}