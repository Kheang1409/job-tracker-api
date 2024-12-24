using AutoMapper;
using JobService.DTOs;
using JobService.Kafka;
using JobService.Models;
using JobService.Repositories;

namespace JobService.Services;

public class JobService : IJobService
{
    private readonly IJobRepository _jobRepository;
    private readonly IMapper _mapper;
    private readonly IKafkaProducer _kafkaProducer;

    public JobService(IJobRepository jobRepository, IMapper mapper, IKafkaProducer kafkaProducer)
    {
        _jobRepository = jobRepository;
        _mapper = mapper;
        _kafkaProducer = kafkaProducer;
    }

    public async Task<Job?> GetJobByIdAsync(string jobId, string userId)
    {
        var job = await _jobRepository.GetJobByIdAsync(jobId);

        if (job == null || job.UserId != userId)
        {
            return null;
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

    public async Task<IEnumerable<Job>> GetJobsByUserIdAsync(string userId)
    {
        var jobs = await _jobRepository.GetJobsByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<Job>>(jobs);
    }

    public async Task<Job> CreateJobAsync(CreateJobDto createJobDto, string userId)
    {
        var job = _mapper.Map<Job>(createJobDto);
        job.UserId = userId;
        job.Status = "Applied";
        await _jobRepository.CreateJobAsync(job);
        return _mapper.Map<Job>(job);
    }

    public async Task<JobDto?> UpdateJobStatusAsync(string jobId, UpdateJobStatusDto updateStatusDto, string userId)
    {
        var job = await _jobRepository.GetJobByIdAsync(jobId);

        if (job == null || job.UserId != userId)
        {
            return null;
        }

        job.Status = updateStatusDto.Status;
        await _jobRepository.UpdateJobAsync(job);

        return _mapper.Map<JobDto>(job);

        throw new ArgumentException("Invalid job status.");
    }

    public async Task<bool> SetInterviewReminderAsync(string jobId, SetInterviewReminderDto reminderDto, string userEmail)
    {
        var job = await _jobRepository.GetJobByIdAsync(jobId);

        if (job == null)
        {
            return false;
        }

        job.InterviewDate = reminderDto.InterviewDate;
        job.ReminderDaysBeforeInterview = reminderDto.ReminderDaysBeforeInterview;

        await _jobRepository.UpdateJobAsync(job);

        if (reminderDto.InterviewDate.HasValue && reminderDto.ReminderDaysBeforeInterview > 0)
        {
            var reminderDate = reminderDto.InterviewDate.Value.AddDays(-reminderDto.ReminderDaysBeforeInterview);

            var reminderNotification = new EmailNotification
            {
                JobId = jobId,
                Email = userEmail,
                ScheduledDate = reminderDate,
                Type = "reminder",
                Message = $"Reminder: Your interview for '{job.Title}' at '{job.Company}' is scheduled on {job.InterviewDate:MMMM dd, yyyy}."
            };
            _kafkaProducer.Produce("job-topic", Guid.NewGuid().ToString(), reminderNotification);
        }

        if (reminderDto.InterviewDate.HasValue)
        {
            var goodLuckNotification = new EmailNotification
            {
                JobId = jobId,
                Email = userEmail,
                ScheduledDate = reminderDto.InterviewDate.Value,
                Type = "goodLuck",
                Message = $"Good Luck on your interview for '{job.Title}' at '{job.Company}'!"
            };
            _kafkaProducer.Produce("job-topic", Guid.NewGuid().ToString(), goodLuckNotification);
        }

        return true;
    }

    public async Task<bool> DeleteJobAsync(string jobId, string userId)
    {
        var job = await _jobRepository.GetJobByIdAsync(jobId);

        if (job == null || job.UserId != userId)
        {
            return false;
        }

        await _jobRepository.DeleteJobAsync(jobId);
        return true;
    }
}