using JobTracker.UserService.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using JobTracker.UserService.Application.Services;

namespace JobTracker.UserService.Infrastructure.Persistence;

internal static class MongoMappings
{
    private static readonly object RegistrationLock = new();
    private static bool _registered;

    public static void Register()
    {
        lock (RegistrationLock)
        {
            if (_registered)
                return;

            BsonClassMap.RegisterClassMap<User>(map =>
            {
                map.AutoMap();
                map.MapIdProperty(user => user.Id).SetSerializer(new StringSerializer(BsonType.ObjectId));
                map.MapField("_skills").SetElementName("Skills");
                map.MapField("_experiences").SetElementName("Experiences");
                map.MapField("_projects").SetElementName("Projects");
                map.UnmapProperty(user => user.Skills);
                map.UnmapProperty(user => user.Experiences);
                map.UnmapProperty(user => user.Projects);
                map.UnmapProperty(user => user.EmailVerificationDeliveryToken);
            });

            BsonClassMap.RegisterClassMap<JobApplication>(map =>
            {
                map.AutoMap();
                map.MapIdProperty(application => application.Id)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
            });

            BsonClassMap.RegisterClassMap<EmailOutboxMessage>(map =>
            {
                map.AutoMap();
                map.UnmapProperty(message => message.Token);
            });

            _registered = true;
        }
    }
}
