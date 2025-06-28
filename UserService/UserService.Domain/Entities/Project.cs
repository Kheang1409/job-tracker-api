using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobTracker.UserService.Domain.Entities;

public class Project
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string About { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public Project() { }

    private Project(string name, string about, DateTime startDate, DateTime endDate)
    {
        Id = ObjectId.GenerateNewId().ToString();
        Name = name;
        About = about;
        StartDate = startDate;
        EndDate = endDate;
    }

    public static Project Create(string name, string about, DateTime startDate, DateTime endDate)
    {
        return new Project(name, about, startDate, endDate);
    }
    public void Update(string name, string about, DateTime startDate, DateTime endDate)
    {
        Name = name;
        About = about;
        StartDate = startDate;
        EndDate = endDate;
    }
}