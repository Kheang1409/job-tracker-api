using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobService.Models
{
    public class Job
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonRequired]
        public string UserId { get; set; }
        [BsonRequired]
        public string Title { get; set; }
        [BsonRequired]
        public string Company { get; set; }
        public string[]? Skills { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string? Description { get; set; }
        public Location? Location { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? AppliedDate { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string? Status { get; set; } = "Saved";
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? InterviewDate { get; set; }
        [BsonRepresentation(BsonType.Int32)]
        public int? ReminderDaysBeforeInterview { get; set; }
    }
}
