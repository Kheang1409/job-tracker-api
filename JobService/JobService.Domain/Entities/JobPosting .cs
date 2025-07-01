using JobTracker.JobService.Domain.Commons;
using JobTracker.JobService.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobTracker.JobService.Domain.Entities;

public class JobPosting
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; } = string.Empty;
    public string AuthorId { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string CompanyName { get; private set; } = string.Empty;
    public WorkMode WorkMode { get; private set; }
    public EmploymentType EmploymentType { get; private set; }
    public int NumberOfOpenings { get; private set; } = 1;
    public int MinExperience { get; private set; }
    public SalaryRange? SalaryRange { get; private set; }
    public List<Skill> RequiredSkills { get; private set; } = new();
    public string JobDescription { get; private set; } = string.Empty;
    public Address? JobLocation { get; private set; }
    public List<Candidate> Candidates { get; private set; } = new();
    public JobPostStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime ExpirationDate { get; private set; }
    public JobPosting() { }

    private JobPosting(
        string authorId,
        string title,
        string companyName,
        WorkMode workMode,
        EmploymentType employmentType,
        int numberOfOpenings,
        int minExperience,
        SalaryRange salaryRange,
        List<Skill> requiredSkills,
        string jobDescription,
        Address jobLocation
    )
    {
        Id = ObjectId.GenerateNewId().ToString();
        AuthorId = authorId;
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
        Status = JobPostStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }
    public static JobPosting Create(
        string authorId,
        string title,
        string companyName,
        WorkMode workMode,
        EmploymentType employmentType,
        int numberOfOpenings,
        int minExperience,
        SalaryRange salaryRange,
        List<Skill> requiredSkills,
        string jobDescription,
        Address jobLocation
    )
    {
        return new JobPosting(
            authorId,
            title,
            companyName,
            workMode,
            employmentType,
            numberOfOpenings,
            minExperience,
            salaryRange,
            requiredSkills,
            jobDescription,
            jobLocation);
    }

    public void Update(
        string title,
        string companyName,
        string workMode,
        string employmentType,
        int numberOfOpenings,
        int minExperience,
        int minSalary,
        int maxSalary,
        string currency ,
        List<Skill> requiredSkills,
        string jobDescription,
        string street,
        string city,
        string state,
        string country,
        int postalCode)
    {
        Title = title;
        CompanyName = companyName;
        WorkMode = EnumParser.WorkMode(workMode);
        EmploymentType = EnumParser.EmploymentType(employmentType);
        NumberOfOpenings = numberOfOpenings;
        MinExperience = minExperience;
        SalaryRange?.Update(minSalary, maxSalary, currency);
        JobLocation?.Update(country, street, city, state, postalCode);
        RequiredSkills = requiredSkills;
        JobDescription = jobDescription;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(JobPostStatus status)
    {
        Status = status;
    }
}
