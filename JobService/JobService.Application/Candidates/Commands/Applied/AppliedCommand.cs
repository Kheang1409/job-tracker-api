using MediatR;

namespace JobTracker.JobService.Application.Candidates.Applied.Commands;

public record AppliedCommand(
    string JobPostId,
    string CandidateId,
    string FirstName,
    string LastName,
    string Email): IRequest<string>;