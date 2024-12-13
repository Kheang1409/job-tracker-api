using JobService.Models;

namespace JobService.Repositories
{
    public interface IJobRepository
    {
        Task<Job> GetJobByIdAsync(string id);
        Task<IEnumerable<Job>> GetJobsByUserIdAsync(string userId);
        Task CreateJobAsync(Job job);
        Task UpdateJobAsync(Job job);
        Task DeleteJobAsync(string id);
    }
}
