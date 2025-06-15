using JobTracker.JobService.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobTracker.JobService.Domain.Entities;

public class JobPosting
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; } = string.Empty;
    public string AutherId { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string CompanyName { get; private set; } = string.Empty;
    public WorkMode WorkMode { get; private set; }
    public EmploymentType EmploymentType { get; private set; }
    public int NumberOfOpenings { get; private set; } = 1;
    public int MinExperience { get; private set; }
    public SalaryRange? SalaryRange { get; private set; }
    public List<Skill> RequiredSkills { get; private set; } = new();
    public string JobDescription { get; private set; } = string.Empty;
    public Location? JobLocation { get; private set; }
    public Status Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime ExpirationDate { get; private set; }
    public JobPosting() { }

    private JobPosting(
        string autherId,
        string title,
        string companyName,
        WorkMode workMode,
        EmploymentType employmentType,
        int numberOfOpenings,
        int minExperience,
        SalaryRange salaryRange,
        List<Skill> requiredSkills,
        string jobDescription,
        Location jobLocation,
        Status status
    )
    {
        Id = ObjectId.GenerateNewId().ToString();
        AutherId = autherId;
        Title = title;
        CompanyName = companyName;
        WorkMode = workMode;
        EmploymentType = employmentType;
        NumberOfOpenings = numberOfOpenings;
        MinExperience = minExperience;
        SalaryRange = salaryRange;
        RequiredSkills = requiredSkills;
        JobDescription = jobDescription;
        JobLocation = jobLocation;
        Status = status;
        CreatedAt = DateTime.UtcNow;
    }
    public static JobPosting Create(
        string autherId,
        string title,
        string companyName,
        WorkMode workMode,
        EmploymentType employmentType,
        int numberOfOpenings,
        int minExperience,
        SalaryRange salaryRange,
        List<Skill> requiredSkills,
        string jobDescription,
        Location jobLocation,
        Status status
    )
    {
        return new JobPosting(
            autherId,
            title,
            companyName,
            workMode,
            employmentType,
            numberOfOpenings,
            minExperience,
            salaryRange,
            requiredSkills,
            jobDescription,
            jobLocation,
            status);
    }

    public void Update(
        string title,
        string companyName,
        int numberOfOpenings,
        int minExperience,
        string jobDescription)
    {
        Title = title;
        CompanyName = companyName;
        NumberOfOpenings = numberOfOpenings;
        MinExperience = minExperience;
        JobDescription = jobDescription;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(Status status)
    {
        Status = status;
    }
}
