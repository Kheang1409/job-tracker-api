using JobService.Models;

namespace JobService.Repositories
{
    public interface IJobRepository
    {
        Task<IEnumerable<Job>> GetJobsAsync(int pageNumber, string status, string sort);
        Task<Job> GetJobByIdAsync(string id);
        Task<int> GetTotalJobsCountAsync(string status);
        Task CreateJobAsync(Job job);
        Task UpdateJobAsync(Job job);
        Task DeleteJobAsync(string id);
    }
}
