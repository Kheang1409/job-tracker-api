using JobTracker.JobApplicationService.Domain.Entities;

namespace JobTracker.JobApplicationService.Application.Repositories;

public interface IJobApplicationRepository
{
    Task<IReadOnlyList<JobApplication>> GetByUserIdAsync(string userId);
    Task<JobApplication?> GetByIdAsync(string userId, string id);
    Task AddAsync(JobApplication application);
    Task UpdateAsync(JobApplication application);
    Task<bool> DeleteAsync(string userId, string id);
}
