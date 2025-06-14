using JobTracker.UserService.Application.CustomExceptions;
using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Entities;
using MongoDB.Driver;

namespace JobTracker.UserService.Infrastructure.Repositories;

public class SkillRepository : ISkillRepository
{

    private readonly IMongoCollection<User> _users;

    public SkillRepository(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("JobTrackerApp");
        _users = database.GetCollection<User>("Users");
    }

    public async Task<Skill> GetByIdAsync(string UserId, string SkillId)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, UserId);
        var projection = Builders<User>.Projection
            .ElemMatch(u => u.Skills, s => s.Id == SkillId)
            .Exclude("_id");

        var result = await _users.Find(filter)
            .Project<UserProjection>(projection)
            .FirstOrDefaultAsync();

        if (result == null || result.Skills == null || !result.Skills.Any())
            throw new NotFoundException("Skill not found");
        return result.Skills.First();
    }
    public async Task<IEnumerable<Skill>> GetAllAsync(string UserId)
    {
        var userFilter = Builders<User>.Filter.Eq(u => u.Id, UserId);
        var user = await _users.Find(userFilter).FirstOrDefaultAsync();
        return user?.Skills ?? Enumerable.Empty<Skill>();
    }

    public async Task<string> AddAsync(string UserId, Skill Skill)
    {
        var findUser = Builders<User>.Filter.Eq(u => u.Id, UserId);
        var addSkill = Builders<User>.Update.Push(u => u.Skills, Skill);

        var result = await _users.UpdateOneAsync(findUser, addSkill);
        if (result.MatchedCount == 0)
            throw new NotFoundException("User not found");
        return Skill.Id;
    }

    public async Task<bool> UpdateAsync(string UserId, Skill Skill)
    {
        var filter = Builders<User>.Filter.And(
            Builders<User>.Filter.Eq(u => u.Id, UserId),
            Builders<User>.Filter.ElemMatch(u => u.Skills, s => s.Id == Skill.Id)
        );

        var update = Builders<User>.Update
            .Set(u => u.Skills[0].Name, Skill.Name);

        var result = await _users.UpdateOneAsync(filter, update);
        if (result.MatchedCount == 0)
            throw new NotFoundException("User or Skill not found");
        return true;
    }

    public async Task<bool> DeleteAsync(string UserId, string SkillId)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, UserId);
        var update = Builders<User>.Update.PullFilter(u => u.Skills, s => s.Id == SkillId);
        var result = await _users.UpdateOneAsync(filter, update);
        if (result.MatchedCount == 0)
            throw new NotFoundException("User or Skill not found");
        return true;
    }

    private class UserProjection
    {
        public List<Skill>? Skills { get; set; }
    }
}