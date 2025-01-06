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

    public async Task<IEnumerable<Job>> GetJobsAsync(int pageNumber, string userId)
    {
        var jobs = await _jobRepository.GetJobsAsync(pageNumber, userId);
        return _mapper.Map<IEnumerable<Job>>(jobs);
    }

    public async Task<Job?> GetJobByIdAsync(string jobId)
    {
        var job = await _jobRepository.GetJobByIdAsync(jobId);

        if (job == null)
        {
            return null;
        }

        return job;
    }

    public async Task<Job> CreateJobAsync(JobDto createJobDto, string userId)
    {
        var job = _mapper.Map<Job>(createJobDto);
        job.UserId = userId;
        await _jobRepository.CreateJobAsync(job);
        return _mapper.Map<Job>(job);
    }

    public async Task<JobDto?> FullUpdateJobAsync(string jobId, JobDto updateDto, string userId)
    {
        var job = await _jobRepository.GetJobByIdAsync(jobId);

        if (job == null || job.UserId != userId)
        {
            return null;
        }

        job.Title = updateDto.Title;
        job.Company = updateDto.Company;
        job.MinExperience = updateDto.MinExperience;
        job.MinSalary = updateDto.MinSalary;
        job.MaxSalary = updateDto.MaxSalary;
        job.Skills = updateDto.Skills;
        job.Description = updateDto.Description;
        job.Location = _mapper.Map<Location>(updateDto.Location);
        job.ModifiedDate = DateTime.UtcNow;

        await _jobRepository.UpdateJobAsync(job);
        return _mapper.Map<JobDto>(job);
    }

    public async Task<JobDto?> PartialUpdateJobAsync(string jobId, JobDto updateDto, string userId)
    {
        var job = await _jobRepository.GetJobByIdAsync(jobId);

        if (job == null || job.UserId != userId)
        {
            return null;
        }

        job.Title = updateDto.Title != null ? updateDto.Title : job.Title;
        job.Company = updateDto.Company != null ? updateDto.Company : job.Company;
        job.MinExperience = updateDto.MinExperience != null ? updateDto.MinExperience : job.MinExperience;
        job.MinSalary = updateDto.MinSalary != null ? updateDto.MinSalary : job.MinSalary;
        job.MaxSalary = updateDto.MaxSalary != null ? updateDto.MaxSalary : job.MaxSalary;
        job.Skills = updateDto.Skills != null ? updateDto.Skills : job.Skills;
        job.Description = updateDto.Description != null ? updateDto.Description : job.Description;
        job.Location = updateDto.Location != null ? _mapper.Map<Location>(updateDto.Location) : job.Location;
        job.ModifiedDate = DateTime.UtcNow;

        await _jobRepository.UpdateJobAsync(job);
        return _mapper.Map<JobDto>(job);
    }

    public async Task<Application> ApplyJobAsync(string jobId, Application application)
    {
        var job = await _jobRepository.GetJobByIdAsync(jobId);

        if (job == null && job.Status != "Active")
        {
            return null;
        }
        var existApplication = job.Applications.Find(_application => _application.UserId == application.UserId);
        if (existApplication == null)
        {
            job.Applications.Add(application);
            existApplication = application;
        }
        await _jobRepository.UpdateJobAsync(job);
        return existApplication;
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

    public async Task<int> GetTotalJobsCountAsync()
    {
        int jobsCount = await _jobRepository.GetTotalJobsCountAsync();
        return jobsCount;
    }

    public async Task<bool> SetInterviewReminderAsync(string jobId, string applicationId, SetInterviewReminderDto reminderDto, string userEmail)
    {
        var job = await _jobRepository.GetJobByIdAsync(jobId);

        if (job == null)
        {
            return false;
        }

        var application = job?.Applications?.Where(_application => _application.Id == applicationId).First();
        if (application == null)
        {
            return false;
        }

        application.InterviewDate = reminderDto?.InterviewDate?.Date;
        application.Status = "Interview";
        await _jobRepository.UpdateJobAsync(job);

        if (reminderDto.InterviewDate.HasValue)
        {
            var reminderDate = reminderDto.InterviewDate.Value.Date;

            var reminderNotification = new EmailNotification
            {
                JobId = jobId,
                Username = application.Username,
                Email = application.Email,
                ScheduledDate = reminderDate,
                Type = "reminder",
                Message = $"your interview for the position of '{job.Title}' at '{job.Company}' is scheduled for {reminderDate:MMMM dd, yyyy}."

            };
            _kafkaProducer.Produce("job-topic", Guid.NewGuid().ToString(), reminderNotification);
        }

        if (reminderDto.InterviewDate.HasValue)
        {
            var goodLuckNotification = new EmailNotification
            {
                JobId = jobId,
                Username = application.Username,
                Email = userEmail,
                ScheduledDate = reminderDto.InterviewDate.Value,
                Type = "goodLuck",
                Message = $"Good Luck on your interview for '{job.Title}' at '{job.Company}'!"
            };
            _kafkaProducer.Produce("job-topic", Guid.NewGuid().ToString(), goodLuckNotification);
        }

        return true;
    }
    public async Task<Application> UpdateApplicationStatusAsync(string jobId, string applicationId, UpdateApplicationStatusDto statusDto, string ownerId)
    {

        var job = await _jobRepository.GetJobByIdAsync(jobId);

        if (job == null && job.UserId != ownerId)
        {
            return null;
        }

        var existApplication = job.Applications.Where(_application => _application.Id == applicationId).First();
        if (existApplication == null)
        {
            return null;
        }

        existApplication.Status = statusDto.Status;
        existApplication.Notes = statusDto.Notes;


        if (existApplication.Status == "Rejected")
        {
            var rejectedNotification = new EmailNotification
            {
                JobId = jobId,
                Username = existApplication.Username,
                Email = existApplication.Email,
                ScheduledDate = existApplication.InterviewDate.Value.Date,
                Type = "rejected",
                Message = $"We regret to inform you that your application for the '{job.Title}' position at '{job.Company}' has not been successful."
            };
            _kafkaProducer.Produce("job-topic", Guid.NewGuid().ToString(), rejectedNotification);
        }

        if (existApplication.Status == "Selected")
        {
            var selectedNotification = new EmailNotification
            {
                JobId = jobId,
                Username = existApplication.Username,
                Email = existApplication.Email,
                ScheduledDate = statusDto.StartDate.Value.Date,
                Type = "selected",
                Message = $"Congratulations! We are excited to inform you that you have been selected for the '{job.Title}' position at '{job.Company}'!"
            };
            _kafkaProducer.Produce("job-topic", Guid.NewGuid().ToString(), selectedNotification);
        }


        await _jobRepository.UpdateJobAsync(job);
        return existApplication;
    }
}