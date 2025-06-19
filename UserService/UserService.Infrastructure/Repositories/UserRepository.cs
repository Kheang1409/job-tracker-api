using JobTracker.UserService.Application.CustomExceptions;
using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Entities;
using MongoDB.Driver;

namespace JobTracker.UserService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("JobTrackerApp");
        _users = database.GetCollection<User>("Users");

        var indexKeys = Builders<User>.IndexKeys.Ascending(u => u.Email);
        var indexOptions = new CreateIndexOptions { Unique = true };
        _users.Indexes.CreateOne(new CreateIndexModel<User>(indexKeys, indexOptions));
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
                filters.Add(filterBuilder.Regex(u => u.FirstName, new MongoDB.Bson.BsonRegularExpression(nameParts[0], "i")));
            }

            if (nameParts.Length > 1)
            {
                filters.Add(filterBuilder.Regex(u => u.LastName, new MongoDB.Bson.BsonRegularExpression(nameParts[1], "i")));
            }
        }

        if (!string.IsNullOrWhiteSpace(Skill))
        {
            filters.Add(filterBuilder.Regex(u => u.Skills, new MongoDB.Bson.BsonRegularExpression(Skill, "i")));
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

    public async Task<User> GetByEmailAsync(string Email) =>
        await _users.Find(u => u.Email == Email).SingleOrDefaultAsync();

    public async Task<User> GetByOTPAsync(string OTP)
    {
        var user = await _users.Find(u => u.OTP == OTP).SingleOrDefaultAsync();
        if (user is null)
            throw new NotFoundException($"The provided OTP '{OTP}' is invalid or has expired.");
        return user;
    }

    public async Task<string> AddAsync(User user) 
    {
        await _users.InsertOneAsync(user);
        return user.Id;
    }


    public async Task<bool> UpdateAsync(User User)
    {
        var result = await _users.ReplaceOneAsync(u => u.Id == User.Id, User);
        if (result.MatchedCount == 0)
            throw new NotFoundException($"User with Id '{User.Id}' not found.");
        return true;
    }

    public async Task<bool> DeleteAsync(string Id)
    {
        var deletedUser = await _users.FindOneAndDeleteAsync(u => u.Id == Id);
        return deletedUser != null;
    }
}
