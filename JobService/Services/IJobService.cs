using JobService.DTOs;
using JobService.Models;

namespace JobService.Services
{
    public interface IJobService
    {
        Task<Job> GetJobByIdAsync(string id, string userId);
        Task<IEnumerable<Job>> GetJobsByUserIdAsync(string userId);
        Task<Job> CreateJobAsync(CreateJobDto createJobDto, string userId);
        Task<JobDto?> UpdateJobStatusAsync(string jobId, UpdateJobStatusDto updateStatusDto, string userId);
        Task<bool> SetInterviewReminderAsync(string jobId, SetInterviewReminderDto reminderDto, string userId);
        Task<bool> DeleteJobAsync(string jobId, string userId);
    }
}
