using AutoMapper;
using JobService.DTOs;
using JobService.Models;
using JobService.Repositories;

namespace JobService.Services;

public class JobService : IJobService
{
    private readonly IJobRepository _jobRepository;
    private readonly IMapper _mapper;

    public JobService(IJobRepository jobRepository, IMapper mapper)
    {
        _jobRepository = jobRepository;
        _mapper = mapper;
    }

    // Get a job by ID for a logged-in user
    public async Task<Job?> GetJobByIdAsync(string jobId, string userId)
    {
        var job = await _jobRepository.GetJobByIdAsync(jobId);

        if (job == null || job.UserId != userId)
        {
            return null; // Job not found or does not belong to the user
        }

        return new Job
        {
            Id = job.Id,
            UserId = job.UserId,
            Title = job.Title,
            Company = job.Company,
            AppliedDate = job.AppliedDate,
            Status = job.Status,
            InterviewDate = job.InterviewDate,
            ReminderDaysBeforeInterview = job.ReminderDaysBeforeInterview
        };
    }

    // Get all jobs created by a logged-in user
    public async Task<IEnumerable<Job>> GetJobsByUserIdAsync(string userId)
    {
        var jobs = await _jobRepository.GetJobsByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<Job>>(jobs);
    }

    // Create a new job application
    public async Task<Job> CreateJobAsync(CreateJobDto createJobDto, string userId)
    {
        var job = _mapper.Map<Job>(createJobDto);
        job.UserId = userId;
        job.Status = "Applied"; // New jobs are initially in the 'Applied' status
        await _jobRepository.CreateJobAsync(job);
        return _mapper.Map<Job>(job);
    }

    // Update job status (applied, interview, interviewed, rejected, accepted)
    public async Task<JobDto?> UpdateJobStatusAsync(string jobId, UpdateJobStatusDto updateStatusDto, string userId)
    {
        var job = await _jobRepository.GetJobByIdAsync(jobId);

        if (job == null || job.UserId != userId)
        {
            return null; // Unauthorized or job not found
        }

        job.Status = updateStatusDto.Status;
        await _jobRepository.UpdateJobAsync(job);

        return _mapper.Map<JobDto>(job);

        throw new ArgumentException("Invalid job status.");
    }

    // Set an interview reminder for a specific job
    public async Task<bool> SetInterviewReminderAsync(string jobId, SetInterviewReminderDto reminderDto, string userId)
    {
        var job = await _jobRepository.GetJobByIdAsync(jobId);

        if (job == null || job.UserId != userId)
        {
            return false; // Job not found or unauthorized access
        }

        job.InterviewDate = reminderDto.InterviewDate;
        job.ReminderDaysBeforeInterview = reminderDto.ReminderDaysBeforeInterview;

        await _jobRepository.UpdateJobAsync(job);

        return true;
    }

    // Delete a job application (only for the logged-in user)
    public async Task<bool> DeleteJobAsync(string jobId, string userId)
    {
        var job = await _jobRepository.GetJobByIdAsync(jobId);

        if (job == null || job.UserId != userId)
        {
            return false; // Unauthorized or job not found
        }

        await _jobRepository.DeleteJobAsync(jobId);
        return true;
    }
}