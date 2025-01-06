using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobService.Models
{
    public class Application
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonRepresentation(BsonType.String)]
        public string UserId { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string Email { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string Username { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string? Status { get; set; } = "Applied";
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? AppliedDate { get; set; } = DateTime.UtcNow;
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? InterviewDate { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string? Notes { get; set; } = "";

    }
}
