using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobService.Models
{
    public class Location
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonRepresentation(BsonType.String)]
        public string? Country { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string? State { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string? City { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string? Address { get; set; }
        [BsonRepresentation(BsonType.Int32)]
        public int? Zip { get; set; }
    }
}