namespace JobTracker.UserService.Domain.Entities;

public class Experience
{
    public string CompanyName { get; private set; } = string.Empty;
    public string Position { get; private set; } = string.Empty;
    public List<string> BulletPoints { get; private set; } = new();
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public bool IsPresent { get; private set; }

    public Experience(Builder builder)
    {
        CompanyName = builder.CompanyName;
        Position = builder.Position;
        BulletPoints = builder.BulletPoints;
        StartDate = builder.StartDate;
        EndDate = builder.EndDate;
        IsPresent = builder.IsPresent;
    }

    public class Builder
    {
        public string CompanyName { get; private set; } = string.Empty;
        public string Position { get; private set; } = string.Empty;
        public List<string> BulletPoints { get; private set; } = new();
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public bool IsPresent { get; private set; }

        public Builder SetCompanyName(string companyName)
        {
            CompanyName = companyName;
            return this;
        }

        public Builder SetPosition(string position)
        {
            Position = position;
            return this;
        }

        public Builder SetBulletPoints(List<string> bulletPoints)
        {
            BulletPoints = bulletPoints;
            return this;
        }
        public Builder SetStartDate(DateTime startDate)
        {
            StartDate = startDate;
            return this;
        }
        public Builder SetEndDate(DateTime? endDate)
        {
            EndDate = endDate;
            return this;
        }
        public Builder SetIsPresent(bool isPresent)
        {
            IsPresent = isPresent;
            return this;
        }
        public Experience Build()
        {
            if (string.IsNullOrEmpty(CompanyName))
                throw new ArgumentNullException(nameof(CompanyName), "CompanyName cannot be null or empty.");
            if (Position is null)
                throw new ArgumentNullException(nameof(Position), "Position cannot be null or empty.");
            if (!StartDate.HasValue)
                throw new ArgumentNullException(nameof(StartDate), "Start date cannot be null or empty.");
            if (!EndDate.HasValue && !IsPresent)
                throw new InvalidOperationException("Either an End Date must be set or the experience must be marked as currently ongoing (IsPresent = true).");
            if (EndDate.HasValue && EndDate.Value < StartDate.Value)
                throw new InvalidOperationException("End date must be later than start date.");
            return new Experience(this);
        }
    }

}