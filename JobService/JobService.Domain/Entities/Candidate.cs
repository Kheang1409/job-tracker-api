using JobTracker.JobService.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobTracker.JobService.Domain.Entities;

public class Candidate
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; } = string.Empty;
    public string CandidateId { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public List<Stage> Rounds { get; private set; } = new();
    public ApplicationStatus Status { get; private set; } = ApplicationStatus.Applied;
    public DateTime AppliedAt { get; private set; }

    public Candidate() { }

    public Candidate(string candidateId, string firstName, string lastName, string email)
    {
        Id = ObjectId.GenerateNewId().ToString();
        CandidateId = candidateId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Rounds.Add(Stage.Create("Resume Review", null));
        AppliedAt = DateTime.UtcNow;
    }

    public void Withdraw()
    {
        Status = ApplicationStatus.Withdrawn;
    }

    public void Apply()
    {
        Status = ApplicationStatus.Applied;
        AppliedAt = DateTime.UtcNow;
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
        Rounds.Add(Stage.Create("Selected", null));
        Rounds.Last().Cleared();
        Status = ApplicationStatus.Selected;
    }
}
