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
        [BsonRepresentation(BsonType.String)]
        public string UserId { get; set; }
        [BsonRequired]
        [BsonRepresentation(BsonType.String)]
        public string Title { get; set; }
        [BsonRequired]
        [BsonRepresentation(BsonType.String)]
        public string Company { get; set; }
        [BsonRepresentation(BsonType.Int32)]
        public int MaxPosition { get; set; } = 1;
        [BsonRepresentation(BsonType.Int32)]
        public int? MinExperience { get; set; }
        [BsonRepresentation(BsonType.Double)]
        public double? MinSalary { get; set; }
        [BsonRepresentation(BsonType.Double)]
        public double? MaxSalary { get; set; }
        public string[]? Skills { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string? Description { get; set; }
        public Location? Location { get; set; }
        public List<Application>? Applications { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string? Status { get; set; } = "Active";
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }

    }
}
