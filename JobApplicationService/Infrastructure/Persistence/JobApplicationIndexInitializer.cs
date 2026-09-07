using JobTracker.JobApplicationService.Domain.Entities;
using MongoDB.Driver;

namespace JobTracker.JobApplicationService.Infrastructure.Persistence;

internal sealed class JobApplicationIndexInitializer(IMongoDatabase database) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var applications = database.GetCollection<JobApplication>("JobApplications");
        return applications.Indexes.CreateOneAsync(
            new CreateIndexModel<JobApplication>(
                Builders<JobApplication>.IndexKeys
                    .Ascending(application => application.UserId)
                    .Descending(application => application.ModifiedAt),
                new CreateIndexOptions { Name = "ix_applications_user_modified" }),
            cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
