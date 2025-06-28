using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobTracker.UserService.Domain.Entities;

public class Experience
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; } = string.Empty;
    public string CompanyName { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
    public Project? Address { get; private set; }
    public List<Skill>? Skills { get; private set; } = new();

}