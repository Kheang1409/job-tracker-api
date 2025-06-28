using JobTracker.SharedKernel.Exceptions;
using JobTracker.UserService.Domain.Entities;
using MongoDB.Driver;

namespace JobTracker.UserService.Application.Repositories;

public class ProjectRepository : IProjectRepository
{

    private readonly IMongoCollection<User> _users;

    public ProjectRepository(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("JobTrackerApp");
        _users = database.GetCollection<User>("Users");
    }
    public async Task<Project> GetByIdAsync(string UserId, string ProjectId)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, UserId);
        var projection = Builders<User>.Projection
            .ElemMatch(u => u.Projects, p => p.Id == ProjectId)
            .Exclude("_id");

        var result = await _users.Find(filter)
            .Project<UserProjection>(projection)
            .FirstOrDefaultAsync();

        if (result == null || result.Projects == null || !result.Projects.Any())
            throw new NotFoundException("Project not found");

        return result.Projects.First();
    }
    public async Task<IEnumerable<Project>> GetAllAsync(string UserId)
    {
        var userFilter = Builders<User>.Filter.Eq(u => u.Id, UserId);
        var user = await _users.Find(userFilter).FirstOrDefaultAsync();
        return user?.Projects ?? Enumerable.Empty<Project>();
    }
    public async Task<string> AddAsync(string UserId, Project Project)
    {
        var findUser = Builders<User>.Filter.Eq(u => u.Id, UserId);
        var addProject = Builders<User>.Update.Push(u => u.Projects, Project);

        var result = await _users.UpdateOneAsync(findUser, addProject);
        if (result.MatchedCount == 0)
            throw new NotFoundException("User not found");
        return Project.Id;
    }
    public async Task<bool> UpdateAsync(string UserId, Project Project)
    {
        var filter = Builders<User>.Filter.And(
            Builders<User>.Filter.Eq(u => u.Id, UserId),
            Builders<User>.Filter.ElemMatch(u => u.Projects, p => p.Id == Project.Id)
        );

        var update = Builders<User>.Update
            .Set(u => u.Projects[0].Name, Project.Name)
            .Set(u => u.Projects[0].About, Project.About)
            .Set(u => u.Projects[0].StartDate, Project.StartDate)
            .Set(u => u.Projects[0].EndDate, Project.EndDate);

        var result = await _users.UpdateOneAsync(filter, update);
        if (result.MatchedCount == 0)
            throw new NotFoundException("User or Project not found");
        return true;
    }
    public async Task<bool> DeleteAsync(string UserId, string ProjectId)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, UserId);
        var update = Builders<User>.Update.PullFilter(u => u.Addresses, s => s.Id == ProjectId);
        var result = await _users.UpdateOneAsync(filter, update);
        if (result.MatchedCount == 0)
            throw new NotFoundException("User or Project not found");
        return true;
    }
    private class UserProjection
    {
        public List<Project>? Projects { get; set; }
    }
}

