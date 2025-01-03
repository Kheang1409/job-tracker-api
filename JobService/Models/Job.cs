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
        public string[] Skills { get; set; }
        public string? Description { get; set; }
        public Location? Location { get; set; }
        public DateTime? AppliedDate { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string Status { get; set; }
        public DateTime? InterviewDate { get; set; }
        public int? ReminderDaysBeforeInterview { get; set; }
    }
}
