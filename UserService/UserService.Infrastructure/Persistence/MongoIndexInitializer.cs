using JobTracker.UserService.Domain.Entities;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using JobTracker.UserService.Application.Services;

namespace JobTracker.UserService.Infrastructure.Persistence;

internal sealed class MongoIndexInitializer(IMongoDatabase database) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var users = database.GetCollection<User>("Users");
        await users.Indexes.CreateManyAsync([
            new CreateIndexModel<User>(
                Builders<User>.IndexKeys.Ascending(user => user.Email),
                new CreateIndexOptions { Unique = true, Name = "ux_users_email" }),
            new CreateIndexModel<User>(
                Builders<User>.IndexKeys.Ascending(user => user.Username),
                new CreateIndexOptions { Unique = true, Sparse = true, Name = "ux_users_username" })
        ], cancellationToken);

        var applications = database.GetCollection<JobApplication>("JobApplications");
        await applications.Indexes.CreateOneAsync(
            new CreateIndexModel<JobApplication>(
                Builders<JobApplication>.IndexKeys
                    .Ascending(application => application.UserId)
                    .Descending(application => application.ModifiedAt),
                new CreateIndexOptions { Name = "ix_applications_user_modified" }),
            cancellationToken: cancellationToken);

        var outbox = database.GetCollection<EmailOutboxMessage>("EmailOutbox");
        await outbox.Indexes.CreateOneAsync(
            new CreateIndexModel<EmailOutboxMessage>(
                Builders<EmailOutboxMessage>.IndexKeys
                    .Ascending(message => message.NextAttemptAt)
                    .Ascending(message => message.LeaseExpiresAt)
                    .Ascending(message => message.CreatedAt),
                new CreateIndexOptions { Name = "ix_email_outbox_dispatch" }),
            cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
