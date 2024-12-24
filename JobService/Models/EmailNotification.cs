using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobService.Models
{
    public class EmailNotification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonRequired]
        public string JobId { get; set; }

        [BsonRequired]
        public string Email { get; set; }

        [BsonRequired]
        public DateTime ScheduledDate { get; set; }

        [BsonRequired]
        [BsonRepresentation(BsonType.String)]
        public string Type { get; set; } // "Reminder" or "GoodLuck"

        public string Message { get; set; } // Optional: Custom message for the email

        public bool IsSent { get; set; } = false; // Track sent status
    }

}