using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Enums;
using JobTracker.SharedKernel.Exceptions;
using JobTracker.SharedKernel.Messaging;
using MediatR;

namespace JobTracker.JobService.Application.Candidates.Selected.Commands;

public class SelectedCommandHandler : IRequestHandler<SelectedCommand, bool>
{
    private readonly IJobPostRepository _jobPostRepository;
    private readonly ICandidateRepository _candidateRepository;
    private readonly IKafkaProducer _kafkaProducer;

    public SelectedCommandHandler(
        IJobPostRepository jobPostRepository,
        ICandidateRepository candidateRepository,
        IKafkaProducer kafkaProducer
    )
    {
        _jobPostRepository = jobPostRepository;
        _candidateRepository = candidateRepository;
        _kafkaProducer = kafkaProducer;
    }
    
    public async Task<bool> Handle(SelectedCommand command, CancellationToken cancellationToken)
    {
        var jobPosting = await _jobPostRepository.GetByIdAsync(command.JobPostId);
        if (jobPosting.AutherId != command.AuthorId)
            throw new UnauthorizedAccessException("User is not authorized");
        var candidate = jobPosting.Candidates.SingleOrDefault(c => c.Id == command.CandidateId && c.Status == ApplicationStatus.Applied)
            ?? throw new NotFoundException("Candidate not found");

        candidate.Selected();
        var notificationPayload = new
            {
                Type = "Selected",
                candidate.Email,
                candidate.FirstName,
                candidate.LastName,
                jobPosting.Title,
                jobPosting.CompanyName,
            };
        await Task.WhenAll(
            _kafkaProducer.Produce("job-tracker-topic", Guid.NewGuid().ToString(), notificationPayload),
            _candidateRepository.UpdateAsync(command.JobPostId, command.CandidateId, candidate)
        );
        return true; 
    }
}