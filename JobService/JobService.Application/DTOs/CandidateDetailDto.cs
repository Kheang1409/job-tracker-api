using JobTracker.JobService.Domain.Entities;

namespace JobTracker.JobService.Application.DTOs;

public class CandidateDetailDto
{
    public string Id { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public IEnumerable<StageDto>? Rounds { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public DateTime AppliedDate { get; private set; }


    public static explicit operator CandidateDetailDto(Candidate candidate)
    {
        return new CandidateDetailDto
        {
            Id = candidate.Id,
            FirstName = candidate.FirstName,
            LastName = candidate.LastName,
            Email = candidate.Email,
            Rounds = candidate.Rounds.Select(s => (StageDto)s),
            Status = candidate.Status.ToString(),
            AppliedDate = candidate.AppliedAt
        };
    }

    public class StageDto
    {
        public string  Name { get; private set; } = string.Empty;
        public DateTime AppointmentDate  { get; private set; }
        public string Remarked { get; private set; } = string.Empty;
        public string Status { get; private set; } = string.Empty;

        public static explicit operator StageDto(Stage stage)
        {
            return new StageDto
            {
                Name = stage.Name,
                AppointmentDate = stage.AppointmentDate,
                Remarked = stage.Remarked,
                Status = stage.Status.ToString()
            };
        }

    }
}