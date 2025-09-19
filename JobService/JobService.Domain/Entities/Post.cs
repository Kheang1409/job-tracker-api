using JobTracker.JobService.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobTracker.JobService.Domain.Entities;

public class Post
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
    public int MaxExperience { get; private set; }
    public decimal MinSalary { get; private set; }
    public decimal MaxSalary { get; private set; }
    public string Currency { get; private set; }
    public List<string> Skills { get; private set; } = new();
    public string Description { get; private set; } = string.Empty;
    public Address? Address { get; private set; }
    public Status Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? ExpirationDate { get; private set; }

    private Post(Builder builder)
    {
        Id = ObjectId.GenerateNewId().ToString();
        AuthorId = builder.AuthorId;
        Title = builder.Title;
        CompanyName = builder.CompanyName;
        WorkMode = builder.WorkMode;
        EmploymentType = builder.EmploymentType;
        NumberOfOpenings = builder.NumberOfOpenings;
        MinExperience = builder.MinExperience;
        MaxExperience = builder.MaxExperience;
        MinSalary = builder.MinSalary;
        MaxSalary = builder.MaxSalary;
        Currency = builder.Currency;
        Skills = builder.Skills;
        Description = builder.Description;
        Address = builder.Address;
        Status = Status.Active;
        CreatedAt = DateTime.UtcNow;
        ExpirationDate = builder?.ExpirationDate;
    }


    public class Builder
    {
        public string AuthorId { get; private set; } = string.Empty;
        public string Title { get; private set; } = string.Empty;
        public string CompanyName { get; private set; } = string.Empty;
        public WorkMode WorkMode { get; private set; }
        public EmploymentType EmploymentType { get; private set; }
        public int NumberOfOpenings { get; private set; } = 1;
        public int MinExperience { get; private set; }
        public int MaxExperience { get; private set; }
        public decimal MinSalary { get; private set; }
        public decimal MaxSalary { get; private set; }
        public string Currency { get; private set; } = string.Empty;
        public List<string> Skills { get; private set; } = new();
        public string Description { get; private set; } = string.Empty;
        public  Address? Address { get; private set; }
        public Status Status { get; private set; }
        public DateTime? ExpirationDate { get; private set; }


        public Builder SetAuthorId(string authorId)
        {
            AuthorId = authorId;
            return this;
        }
        public Builder SetTitle(string title)
        {
            Title = title;
            return this;
        }
        public Builder SetCompanyName(string companyName)
        {
            CompanyName = companyName;
            return this;
        }
        public Builder SetWorkMode(WorkMode workMode)
        {
            WorkMode = workMode;
            return this;
        }
        public Builder SetEmploymentType(EmploymentType employmentType)
        {
            EmploymentType = employmentType;
            return this;
        }
        public Builder SetNumberOfOpenings(int numberOfOpenings)
        {
            NumberOfOpenings = numberOfOpenings;
            return this;
        }
        public Builder SetMinExperience(int minExperience)
        {
            MinExperience = minExperience;
            return this;
        }
        public Builder SetMaxExperience(int maxExperience)
        {
            MaxExperience = maxExperience;
            return this;
        }
        public Builder SetMinSalary(decimal minSalary)
        {
            MinSalary = minSalary;
            return this;
        }
        public Builder SetMaxSalary(decimal maxSalary)
        {
            MaxSalary = maxSalary;
            return this;
        }
        public Builder SetCurrency(string currency)
        {
            Currency = currency;
            return this;
        }
        public Builder SetSkills(List<string> skills)
        {
            Skills = skills;
            return this;
        }
        public Builder SetDescription(string description)
        {
            Description = description;
            return this;
        }

        public Builder SetAddress(Address? address)
        {
            Address = address;
            return this;
        }
        public Builder SetExpirationDate(DateTime? expirationDate)
        {
            ExpirationDate = expirationDate;
            return this;
        }

        public Post Build()
        {
            if (string.IsNullOrEmpty(AuthorId))
                throw new ArgumentNullException(nameof(AuthorId), "Author Id cannot be null or empty.");

            if (string.IsNullOrEmpty(Title))
                throw new ArgumentNullException(nameof(Title), "Title cannot be null or empty.");

            if (string.IsNullOrEmpty(CompanyName))
                throw new ArgumentNullException(nameof(CompanyName), "Company Name cannot be null or empty.");

            if (MinSalary < 0)
                throw new ArgumentOutOfRangeException(nameof(MinSalary), "Minimum Salary cannot be negative.");

            if (MaxSalary < 0)
                throw new ArgumentOutOfRangeException(nameof(MaxSalary), "Maximum Salary cannot be negative.");

            if (MinSalary > MaxSalary)
                throw new ArgumentException("Minimum Salary cannot be greater than Maximum Salary.");

            if (MinExperience < 0)
                throw new ArgumentOutOfRangeException(nameof(MinExperience), "Minimum Experience cannot be negative.");

            if (MaxExperience < 0)
                throw new ArgumentOutOfRangeException(nameof(MaxExperience), "Maximum Experience cannot be negative.");

            if (MinExperience > MaxExperience)
                throw new ArgumentException("Minimum Experience cannot be greater than Maximum Experience.");

            if (string.IsNullOrEmpty(Currency))
                throw new ArgumentNullException(nameof(Currency), "Currency cannot be null or empty.");

            if (Skills == null || Skills.Count == 0)
                throw new ArgumentException(nameof(Skills), "Skills cannot be null or empty.");

            if (Address == null)
                throw new ArgumentNullException(nameof(Address), "Address cannot be null.");

            if (ExpirationDate.HasValue && ExpirationDate.Value <= DateTime.UtcNow)
                throw new ArgumentException(nameof(ExpirationDate), "Expiration Date must be in the future.");

            return new Post(this);
        }
    }

    public void UpdatePost(
        string title,
        string companyName,
        WorkMode workMode,
        EmploymentType employmentType,
        int numberOfOpenings,
        int minExperience,
        int maxExperience,
        decimal minSalary,
        decimal maxSalary,
        string currency,
        List<string> skills,
        string description,
        Address address,
        DateTime? expirationDate,
        Status status)
    {
        Title = title;
        CompanyName = companyName;
        WorkMode = workMode;
        EmploymentType = employmentType;
        NumberOfOpenings = numberOfOpenings;
        MinExperience = minExperience;
        MaxExperience = maxExperience;
        MinSalary = minSalary;
        MaxSalary = maxSalary;
        Currency = currency;
        Address = address;
        Skills = skills;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
        ExpirationDate = expirationDate;
        Status = status;
    }

    public void SetStatus(Status status)
    {
        Status = status;
    }
}
