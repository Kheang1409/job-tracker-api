using JobTracker.JobService.Application.Infrastructure.Messaging;
using JobTracker.JobService.Application.CustomExceptions;
using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Enums;
using MediatR;
using MongoDB.Bson;

namespace JobTracker.JobService.Application.Candidates.Rejected.Commands;

public class RejectedCommandHandler : IRequestHandler<RejectedCommand, bool>
{
    private readonly IJobPostRepository _jobPostRepository;
    private readonly ICandidateRepository _candidateRepository;
    private readonly IKafkaProducer _kafkaProducer;

    public RejectedCommandHandler(
        IJobPostRepository jobPostRepository,
        ICandidateRepository candidateRepository,
        IKafkaProducer kafkaProducer
    )
    {
        _jobPostRepository = jobPostRepository;
        _candidateRepository = candidateRepository;
        _kafkaProducer = kafkaProducer;
    }
    
    public async Task<bool> Handle(RejectedCommand command, CancellationToken cancellationToken)
    {
        var jobPosting = await _jobPostRepository.GetByIdAsync(command.JobPostId);
        if (jobPosting.AutherId != command.AuthorId)
            throw new UnauthorizedAccessException("User is not authorized");
        var candidate = jobPosting.Candidates.SingleOrDefault(c => c.Id == command.CandidateId && c.Status == ApplicationStatus.Applied)
            ?? throw new NotFoundException("Candidate not found");

        candidate.Rejected();
        var notificationPayload = new
            {
                Type = "Rejected",
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