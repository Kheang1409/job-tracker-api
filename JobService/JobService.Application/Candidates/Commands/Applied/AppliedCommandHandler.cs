using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Entities;
using JobTracker.JobService.Domain.Enums;
using JobTracker.SharedKernel.Exceptions;
using JobTracker.SharedKernel.Messaging;
using MediatR;

namespace JobTracker.JobService.Application.Candidates.Applied.Commands;

public class AppliedCommandHandler : IRequestHandler<AppliedCommand, string>
{
    private readonly IJobPostRepository _jobPostRepository;
    private readonly ICandidateRepository _candidateRepository;
    private readonly IKafkaProducer _kafkaProducer;

    public AppliedCommandHandler(
        IJobPostRepository jobPostRepository,
        ICandidateRepository candidateRepository,
        IKafkaProducer kafkaProducer
    )
    {
        _jobPostRepository = jobPostRepository;
        _candidateRepository = candidateRepository;
        _kafkaProducer = kafkaProducer;
    }
    
    public async Task<string> Handle(AppliedCommand command, CancellationToken cancellationToken)
    {
        var jobPosting = await _jobPostRepository.GetByIdAsync(command.JobPostId);
        if (jobPosting is null)
            throw new NotFoundException("JobPosting not found");
        if (jobPosting.Status == JobPostStatus.Closed)
            throw new ArgumentException("Job post has already closed.");
        if (jobPosting.Candidates.Any(c => c.Id == command.CandidateId))
            throw new ArgumentException("Candidate has already applied to this job post.");
        
        var candidate = new Candidate(command.CandidateId, command.FirstName, command.LastName, command.Email);
        var notificationPayload = new
            {
                Type = "Applied",
                command.Email,
                command.FirstName,
                command.LastName,
                jobPosting.Title,
                jobPosting.CompanyName,
            };
        await Task.WhenAll(
            _kafkaProducer.Produce("job-tracker-topic", Guid.NewGuid().ToString(), notificationPayload),
            _candidateRepository.AddAsync(command.JobPostId, candidate)
        );
        return command.CandidateId;
    }
}