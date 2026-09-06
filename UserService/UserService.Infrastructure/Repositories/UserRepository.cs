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

    public async Task<int> GetUserCountAsync(
        string Fullname,
        string Skill
    )
    {
        var filterBuilder = Builders<User>.Filter;
        var filters = new List<FilterDefinition<User>>();

        if (!string.IsNullOrWhiteSpace(Fullname))
        {
            var nameParts = Fullname.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length > 0)
            {
                filters.Add(filterBuilder.Regex(u => u.FirstName, CaseInsensitiveLiteral(nameParts[0])));
            }

            if (nameParts.Length > 1)
            {
                filters.Add(filterBuilder.Regex(u => u.LastName, CaseInsensitiveLiteral(nameParts[1])));
            }
        }

        if (!string.IsNullOrWhiteSpace(Skill))
        {
            filters.Add(filterBuilder.Regex(
                new StringFieldDefinition<User>("Skills"),
                CaseInsensitiveLiteral(Skill)));
        }

        var combinedFilter = filters.Count > 0 ? filterBuilder.And(filters) : FilterDefinition<User>.Empty;
        var count = await _users.CountDocumentsAsync(combinedFilter);
        return (int)count;
    }

    public async Task<IEnumerable<User>> GetAllAsync(string Fullname, string Skill, int PageNumber, int Limit)
    {
        var filterBuilder = Builders<User>.Filter;
        var filters = new List<FilterDefinition<User>>();

        if (!string.IsNullOrWhiteSpace(Fullname))
        {
            var nameParts = Fullname.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length > 0)
            {
                filters.Add(filterBuilder.Regex(u => u.FirstName, CaseInsensitiveLiteral(nameParts[0])));
            }

            if (nameParts.Length > 1)
            {
                filters.Add(filterBuilder.Regex(u => u.LastName, CaseInsensitiveLiteral(nameParts[1])));
            }
        }

        if (!string.IsNullOrWhiteSpace(Skill))
        {
            filters.Add(filterBuilder.Regex(
                new StringFieldDefinition<User>("Skills"),
                CaseInsensitiveLiteral(Skill)));
        }

        var combinedFilter = filters.Count > 0 ? filterBuilder.And(filters) : FilterDefinition<User>.Empty;

        var skip = (PageNumber - 1) * Limit;

        var users = await _users.Find(combinedFilter)
                                    .Skip(skip)
                                    .Limit(Limit)
                                    .ToListAsync();
        return users;
    }

    public async Task<User> GetByIdAsync(string Id)
    {
        var user = await _users.Find(u => u.Id == Id).SingleOrDefaultAsync();
        if (user is null)
            throw new NotFoundException($"The User unavaliable.");
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


    public async Task<bool> UpdateAsync(User User)
    {
        var result = await _users.ReplaceOneAsync(u => u.Id == User.Id, User);
        if (result.MatchedCount == 0)
            throw new NotFoundException($"User with Id '{User.Id}' not found.");
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

    public async Task<bool> DeleteAsync(string Id)
    {
        var deletedUser = await _users.FindOneAndDeleteAsync(u => u.Id == Id);
        return deletedUser != null;
    }

    private static BsonRegularExpression CaseInsensitiveLiteral(string value) =>
        new(Regex.Escape(value.Trim()), "i");
}
