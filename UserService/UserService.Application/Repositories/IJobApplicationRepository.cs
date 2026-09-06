using JobTracker.UserService.Domain.Entities;

namespace JobTracker.UserService.Application.Repositories;

public interface IJobApplicationRepository
{
    Task<IReadOnlyList<JobApplication>> GetByUserIdAsync(string userId);
    Task<JobApplication?> GetByIdAsync(string userId, string id);
    Task AddAsync(JobApplication application);
    Task UpdateAsync(JobApplication application);
    Task<bool> DeleteAsync(string userId, string id);
}
