using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobTracker.JobService.Domain.Entities;

public class Skill
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public Skill() { }

    private Skill(string name)
    {
        Id = ObjectId.GenerateNewId().ToString();
        Name = name;
    }

    public static Skill Create(string name)
    {
        return new Skill(name);
    }

    public void Rename(string name)
    {
        Name = name;
    }
}