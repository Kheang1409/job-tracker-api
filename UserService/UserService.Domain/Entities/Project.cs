namespace JobTracker.UserService.Domain.Entities;

public class Project
{
    public string Name { get; private set; } = string.Empty;
    public string About { get; private set; } = string.Empty;
    public DateTime? During  { get; private set; }
    public string Link { get; private set; } = string.Empty;

    private Project(Builder builder)
    {
        Name = builder.Name;
        About = builder.About;
        During = builder.During;
        Link = builder.Link;
    }

    public class Builder
    {
        public string Name { get; private set; } = string.Empty;
        public string About { get; private set; } = string.Empty;
        public DateTime? During { get; private set; }
        public string Link { get; private set; } = string.Empty;

        public Builder SetName(string name)
        {
            Name = name;
            return this;
        }

        public Builder SetAbout(string about)
        {
            About = about;
            return this;
        }

        public Builder SetDuring(DateTime during)
        {
            During = during;
            return this;
        }


        public Builder SetLink(string link)
        {
            Link = link;
            return this;
        }

        public Project Build()
        {
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException(nameof(Name), "Name cannot be null or empty.");
            if (During is null)
                throw new ArgumentNullException(nameof(During), "During cannot be null or empty.");
            return new Project(this);
        }
    }
    public void UpdateProject(string name, string about, DateTime during, string link)
    {
        Name = name;
        About = about;
        During = during;
        Link = link;
    }
}