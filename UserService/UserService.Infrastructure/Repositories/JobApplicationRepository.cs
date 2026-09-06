using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Entities;
using MongoDB.Driver;

namespace JobTracker.UserService.Infrastructure.Repositories;

public sealed class JobApplicationRepository : IJobApplicationRepository
{
    private readonly IMongoCollection<JobApplication> _applications;

    public JobApplicationRepository(IMongoDatabase database)
    {
        _applications = database.GetCollection<JobApplication>("JobApplications");
    }

    public async Task<IReadOnlyList<JobApplication>> GetByUserIdAsync(string userId) =>
        await _applications.Find(application => application.UserId == userId)
            .SortByDescending(application => application.ModifiedAt)
            .ToListAsync();

    public async Task<JobApplication?> GetByIdAsync(string userId, string id) =>
        await _applications.Find(application => application.UserId == userId && application.Id == id)
            .SingleOrDefaultAsync();

    public Task AddAsync(JobApplication application) =>
        _applications.InsertOneAsync(application);

    public async Task UpdateAsync(JobApplication application)
    {
        var result = await _applications.ReplaceOneAsync(
            existing => existing.UserId == application.UserId && existing.Id == application.Id,
            application);
        if (result.MatchedCount == 0)
            throw new InvalidOperationException("The job application was not found.");
    }

    public async Task<bool> DeleteAsync(string userId, string id)
    {
        var result = await _applications.DeleteOneAsync(
            application => application.UserId == userId && application.Id == id);
        return result.DeletedCount > 0;
    }
}
