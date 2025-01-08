using JobService.DTOs;
using JobService.Models;

namespace JobService.Services
{
    public interface IJobService
    {
        Task<IEnumerable<Job>> GetJobsAsync(int pageNumber, string status, string sort);
        Task<Job> GetJobByIdAsync(string id);
        Task<Job> CreateJobAsync(JobDto createJobDto, string ownerId);
        Task<JobDto?> FullUpdateJobAsync(string jobId, JobDto updateJobDto, string ownerId);
        Task<JobDto?> PartialUpdateJobAsync(string jobId, JobDto updateJobDto, string ownerId);
        Task<Job?> UpdateJobStatus(string jodId, string ownerId);
        Task<Application> ApplyJobAsync(string jobId, Application application);
        Task<bool> DeleteJobAsync(string jobId, string ownerId);
        Task<int> GetTotalJobsCountAsync(string status);
        Task<bool> SetInterviewReminderAsync(string jobId, string applicationId, SetInterviewReminderDto reminderDto, string userEmail);
        Task<Application> UpdateApplicationStatusAsync(string jobId, string applicationId, UpdateApplicationStatusDto statusDto, string ownerId);

    }
}
