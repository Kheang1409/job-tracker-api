using JobTracker.UserService.Application.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace JobTracker.UserService.Infrastructure.Services;

public sealed class EmailOutboxPublisher(
    IMongoDatabase database,
    EmailServiceClient emailClient,
    EmailOutboxProtector protector,
    ILogger<EmailOutboxPublisher> logger) : BackgroundService
{
    private readonly IMongoCollection<EmailOutboxMessage> _outbox =
        database.GetCollection<EmailOutboxMessage>("EmailOutbox");

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            EmailOutboxMessage? message;
            try
            {
                message = await ClaimNextAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Reading the email outbox failed; retrying.");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                continue;
            }
            if (message is null)
            {
                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                continue;
            }

            try
            {
                var token = protector.Unprotect(message);
                await emailClient.PublishAsync(message, token, stoppingToken);
                await _outbox.DeleteOneAsync(item => item.Id == message.Id, stoppingToken);
                logger.LogInformation("Published email outbox message {MessageId}.", message.Id);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                var delay = TimeSpan.FromSeconds(Math.Min(Math.Pow(2, message.Attempts), 300));
                var update = Builders<EmailOutboxMessage>.Update
                    .Set(item => item.NextAttemptAt, DateTime.UtcNow.Add(delay))
                    .Set(item => item.LeaseExpiresAt, null)
                    .Set(item => item.LastError, exception.GetType().Name);
                await _outbox.UpdateOneAsync(item => item.Id == message.Id, update, cancellationToken: stoppingToken);
                logger.LogError(exception, "Publishing outbox message {MessageId} failed; retrying in {RetryDelay}.", message.Id, delay);
            }
        }
    }

    private async Task<EmailOutboxMessage?> ClaimNextAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var filter = Builders<EmailOutboxMessage>.Filter.And(
            Builders<EmailOutboxMessage>.Filter.Lte(item => item.NextAttemptAt, now),
            Builders<EmailOutboxMessage>.Filter.Or(
                Builders<EmailOutboxMessage>.Filter.Eq(item => item.LeaseExpiresAt, null),
                Builders<EmailOutboxMessage>.Filter.Lt(item => item.LeaseExpiresAt, now)));
        var update = Builders<EmailOutboxMessage>.Update
            .Set(item => item.LeaseExpiresAt, now.AddMinutes(1))
            .Inc(item => item.Attempts, 1);
        return await _outbox.FindOneAndUpdateAsync(
            filter,
            update,
            new FindOneAndUpdateOptions<EmailOutboxMessage>
            {
                Sort = Builders<EmailOutboxMessage>.Sort.Ascending(item => item.CreatedAt),
                ReturnDocument = ReturnDocument.After
            },
            cancellationToken);
    }
}
