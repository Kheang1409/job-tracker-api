using JobTracker.UserService.Domain.Entities;
using JobTracker.UserService.Infrastructure.Persistence;
using MongoDB.Bson;

namespace JobTracker.UserService.UnitTests;

public sealed class MongoMappingTests
{
    [Fact]
    public void User_RoundTripsWithoutPersistenceAttributesInDomain()
    {
        MongoMappings.Register();
        var user = new User.Builder()
            .SetUsername("tester")
            .SetFirstName("Test")
            .SetLastName("User")
            .SetEmail("user@example.com")
            .SetPassword("SecurePass1")
            .SetSkills(["C#", "Angular"])
            .Build();

        var document = user.ToBsonDocument();
        var restored = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<User>(document);

        Assert.Equal(user.Id, restored.Id);
        Assert.Equal(user.Email, restored.Email);
        Assert.Equal(["C#", "Angular"], restored.Skills);
    }
}
