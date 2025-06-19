using JobTracker.JobService.Application.CustomExceptions;
using JobTracker.JobService.Application.Infrastructure.Messaging;
using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Enums;
using MediatR;

namespace JobTracker.JobService.Application.Candidates.MoveOn.Commands;

public class MoveOnCommandHandler : IRequestHandler<MoveOnWithIdCommand, bool>
{

    private readonly IJobPostRepository _jobPostRepository;
    private readonly ICandidateRepository _candidateRepository;
    private readonly IKafkaProducer _kafkaProducer;


    public MoveOnCommandHandler(
        IJobPostRepository jobPostRepository,
        ICandidateRepository candidateRepository,
        IKafkaProducer kafkaProducer
    )
    {
        _jobPostRepository = jobPostRepository;
        _candidateRepository = candidateRepository;
        _kafkaProducer = kafkaProducer;
    }

    public async Task<bool> Handle(MoveOnWithIdCommand command, CancellationToken cancellationToken)
    {
        var jobPosting = await _jobPostRepository.GetByIdAsync(command.JobPostId);
        if (jobPosting.AutherId != command.AuthorId)
            throw new UnauthorizedAccessException("User is not authorized");
        var candidate = jobPosting.Candidates.SingleOrDefault(c => c.Id == command.CandidateId && c.Status == ApplicationStatus.Applied)
             ?? throw new NotFoundException("Candidate not found");
        candidate.MoveOn(command.Name, command.AppointmentDate);
        var notificationPayload = new
        {
            Type = "Move On",
            candidate.Email,
            candidate.FirstName,
            candidate.LastName,
            jobPosting.Title,
            jobPosting.CompanyName,
            Stage = command.Name,
            command.AppointmentDate
        };

        await Task.WhenAll(
            _kafkaProducer.Produce("job-tracker-topic", Guid.NewGuid().ToString(), notificationPayload),
            _candidateRepository.UpdateAsync(command.JobPostId, command.CandidateId, candidate)
        );
        return true;
    }
}