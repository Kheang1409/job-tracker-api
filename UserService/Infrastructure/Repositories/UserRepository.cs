using JobTracker.SharedKernel.Exceptions;
using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using JobTracker.UserService.Application.Services;
using JobTracker.UserService.Infrastructure.Services;

namespace JobTracker.UserService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;
    private readonly IMongoCollection<EmailOutboxMessage> _emailOutbox;
    private readonly IMongoClient _client;
    private readonly EmailOutboxProtector _outboxProtector;

    public UserRepository(IMongoDatabase database, EmailOutboxProtector outboxProtector)
    {
        _users = database.GetCollection<User>("Users");
        _emailOutbox = database.GetCollection<EmailOutboxMessage>("EmailOutbox");
        _client = database.Client;
        _outboxProtector = outboxProtector;
    }

    public async Task<int> GetUserCountAsync(string fullname, string skill)
    {
        var count = await _users.CountDocumentsAsync(BuildSearchFilter(fullname, skill));
        return (int)count;
    }

    public async Task<IEnumerable<User>> GetAllAsync(string fullname, string skill, int pageNumber, int limit)
    {
        var skip = (pageNumber - 1) * limit;
        return await _users.Find(BuildSearchFilter(fullname, skill))
            .Skip(skip)
            .Limit(limit)
            .ToListAsync();
    }

    public async Task<User> GetByIdAsync(string id)
    {
        var user = await _users.Find(candidate => candidate.Id == id).SingleOrDefaultAsync();
        if (user is null)
            throw new NotFoundException($"User with Id '{id}' was not found.");
        return user;
    }

    public async Task<User?> GetByEmailAsync(string email) =>
        await _users.Find(u => u.Email == email.Trim().ToLowerInvariant()).SingleOrDefaultAsync();

    public async Task<User?> GetByUsernameAsync(string username) =>
        await _users.Find(u => u.Username == username.Trim()).SingleOrDefaultAsync();

    public async Task<string> AddAsync(User user) 
    {
        await _users.InsertOneAsync(user);
        return user.Id;
    }

    public async Task<string> AddWithEmailAsync(
        User user,
        EmailOutboxMessage email,
        CancellationToken cancellationToken)
    {
        _outboxProtector.Protect(email);
        using var session = await _client.StartSessionAsync(cancellationToken: cancellationToken);
        session.StartTransaction();
        try
        {
            await _users.InsertOneAsync(session, user, cancellationToken: cancellationToken);
            await _emailOutbox.InsertOneAsync(session, email, cancellationToken: cancellationToken);
            await session.CommitTransactionAsync(cancellationToken);
            return user.Id;
        }
        catch
        {
            if (session.IsInTransaction)
                await session.AbortTransactionAsync(CancellationToken.None);
            throw;
        }
    }


    public async Task<bool> UpdateAsync(User user)
    {
        var result = await _users.ReplaceOneAsync(candidate => candidate.Id == user.Id, user);
        if (result.MatchedCount == 0)
            throw new NotFoundException($"User with Id '{user.Id}' not found.");
        return true;
    }

    public async Task<bool> UpdateWithEmailAsync(
        User user,
        EmailOutboxMessage email,
        CancellationToken cancellationToken)
    {
        _outboxProtector.Protect(email);
        using var session = await _client.StartSessionAsync(cancellationToken: cancellationToken);
        session.StartTransaction();
        try
        {
            var result = await _users.ReplaceOneAsync(
                session, existing => existing.Id == user.Id, user, cancellationToken: cancellationToken);
            if (result.MatchedCount == 0)
                throw new NotFoundException($"User with Id '{user.Id}' not found.");
            await _emailOutbox.InsertOneAsync(session, email, cancellationToken: cancellationToken);
            await session.CommitTransactionAsync(cancellationToken);
            return true;
        }
        catch
        {
            if (session.IsInTransaction)
                await session.AbortTransactionAsync(CancellationToken.None);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var deletedUser = await _users.FindOneAndDeleteAsync(user => user.Id == id);
        return deletedUser != null;
    }

    private static FilterDefinition<User> BuildSearchFilter(string fullname, string skill)
    {
        var builder = Builders<User>.Filter;
        var filters = new List<FilterDefinition<User>>();
        var nameParts = fullname?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? [];

        if (nameParts.Length > 0)
            filters.Add(builder.Regex(user => user.FirstName, CaseInsensitiveLiteral(nameParts[0])));
        if (nameParts.Length > 1)
            filters.Add(builder.Regex(user => user.LastName, CaseInsensitiveLiteral(nameParts[1])));
        if (!string.IsNullOrWhiteSpace(skill))
            filters.Add(builder.Regex(new StringFieldDefinition<User>("Skills"), CaseInsensitiveLiteral(skill)));

        return filters.Count > 0 ? builder.And(filters) : FilterDefinition<User>.Empty;
    }

    private static BsonRegularExpression CaseInsensitiveLiteral(string value) =>
        new(Regex.Escape(value.Trim()), "i");
}
