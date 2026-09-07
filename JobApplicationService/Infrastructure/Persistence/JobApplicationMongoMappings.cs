using JobTracker.JobApplicationService.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace JobTracker.JobApplicationService.Infrastructure.Persistence;

internal static class JobApplicationMongoMappings
{
    private static readonly object RegistrationLock = new();
    private static bool _registered;

    internal static void Register()
    {
        lock (RegistrationLock)
        {
            if (_registered) return;
            BsonClassMap.RegisterClassMap<JobApplication>(map =>
            {
                map.AutoMap();
                map.MapIdProperty(application => application.Id)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
            });
            _registered = true;
        }
    }
}
