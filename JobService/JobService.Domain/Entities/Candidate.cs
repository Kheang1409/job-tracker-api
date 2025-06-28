using JobTracker.JobService.Domain.Enums;

namespace JobTracker.JobService.Domain.Entities;

public class Candidate
{
    public string Id { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public List<Stage> Rounds { get; private set; } = new();
    public ApplicationStatus Status { get; private set; } = ApplicationStatus.Applied;
    public DateTime AppliedAt { get; private set; }

    public Candidate() { }

    public Candidate(string id, string firstName, string lastName, string email)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Rounds.Add(Stage.Create("Resume Review"));
        AppliedAt = DateTime.UtcNow;
    }

    public void Withdraw()
    {
        Status = ApplicationStatus.Withdrawn;
    }

    public void MoveOn(string name, DateTime dateTime)
    {
        Rounds.Last().Cleared();
        Rounds.Add(Stage.Create(name, dateTime));
    }

    public void Rejected()
    {
        Rounds.Last().Rejected();
        Status = ApplicationStatus.Rejected;
    }

    public void Selected()
    {
        Rounds.Last().Cleared();
        Status = ApplicationStatus.Selected;
    }
}
