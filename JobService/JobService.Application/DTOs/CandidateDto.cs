using JobTracker.JobService.Domain.Entities;

namespace JobTracker.JobService.Application.DTOs;

public class CandidateDto
{
    public string Id { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string CurrentRound { get; private set; } = string.Empty;
    public string Status { get; private set; } = string.Empty;
    public DateTime AppliedDate { get; private set; }


    public static explicit operator CandidateDto(Candidate candidate)
    {
        return new CandidateDto
        {
            Id = candidate.Id,
            FirstName = candidate.FirstName,
            LastName = candidate.LastName,
            Email = candidate.Email,
            CurrentRound = candidate.Rounds.Last().Name,
            Status = candidate.Status.ToString(),
            AppliedDate = candidate.AppliedAt
        };
    }
}