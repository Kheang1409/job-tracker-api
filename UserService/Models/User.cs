using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserService.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonRepresentation(BsonType.String)]
        public string Username { get; set; }
        [BsonRequired]
        [BsonRepresentation(BsonType.String)]
        public string Email { get; set; }
        [BsonRequired]
        [BsonRepresentation(BsonType.String)]
        public string PasswordHash { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string? OPT { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? OPTExpiry { get; set; }
    }
}