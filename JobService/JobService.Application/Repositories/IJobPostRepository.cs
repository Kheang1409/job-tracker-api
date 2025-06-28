using JobTracker.JobService.Domain.Entities;

namespace JobTracker.JobService.Application.Repositories;
public interface IJobPostRepository
{
    Task<JobPosting> GetByIdAsync(string Id);
    Task<IEnumerable<JobPosting>> GetAllAsync(string Title, string CompanyName, int PageNumber, int Limit);
    Task<string> AddAsync(JobPosting JobPosting);
    Task<bool> UpdateAsync(JobPosting JobPosting);
    Task<bool> DeleteAsync(string Id);
}