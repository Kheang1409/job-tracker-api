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

    public async Task<IEnumerable<Job>> GetJobsAsync(int pageNumber, string status, string sort)
    {
        var jobs = await _jobRepository.GetJobsAsync(pageNumber, status, sort);
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

    public async Task<Job> CreateJobAsync(JobDto createJobDto, string ownerId)
    {
        var job = _mapper.Map<Job>(createJobDto);
        job.UserId = ownerId;
        await _jobRepository.CreateJobAsync(job);
        return _mapper.Map<Job>(job);
    }

    public async Task<JobDto?> FullUpdateJobAsync(string jobId, JobDto updateDto, string ownerId)
    {
        var job = await _jobRepository.GetJobByIdAsync(jobId);

        if (job == null || job.UserId != ownerId)
        {
            return null;
        }

        job.Title = updateDto.Title;
        job.Company = updateDto.Company;
        job.MaxPosition = updateDto.MaxPosition ?? 1;
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

    public async Task<JobDto?> PartialUpdateJobAsync(string jobId, JobDto updateDto, string ownerId)
    {
        var job = await _jobRepository.GetJobByIdAsync(jobId);

        if (job == null || job.UserId != ownerId)
        {
            return null;
        }

        job.Title = updateDto.Title ?? job.Title;
        job.Company = updateDto.Company ?? job.Company;
        job.MaxPosition = updateDto.MaxPosition ?? job.MaxPosition;
        job.MinExperience = updateDto.MinExperience ?? job.MinExperience;
        job.MinSalary = updateDto.MinSalary ?? job.MinSalary;
        job.MaxSalary = updateDto.MaxSalary ?? job.MaxSalary;
        job.Skills = updateDto.Skills ?? job.Skills;
        job.Description = updateDto.Description ?? job.Description;
        job.Location = updateDto.Location != null ? _mapper.Map<Location>(updateDto.Location) : job.Location;
        job.ModifiedDate = DateTime.UtcNow;

        await _jobRepository.UpdateJobAsync(job);
        return _mapper.Map<JobDto>(job);
    }

    public async Task<Job> UpdateJobStatus(string jobId, string ownerId)
    {
        var job = await _jobRepository.GetJobByIdAsync(jobId);

        if (job == null || job.UserId != ownerId)
        {
            return null;
        }
        job.Status = job.Status == "Active" ? "Inactive" : "Active";
        job.ModifiedDate = DateTime.UtcNow;
        await _jobRepository.UpdateJobAsync(job);
        return job;
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

    public async Task<int> GetTotalJobsCountAsync(string status)
    {
        int jobsCount = await _jobRepository.GetTotalJobsCountAsync(status);
        return jobsCount;
    }

    public async Task<bool> UpdateApplicationStatusAsync(string jobId, string applicationId, UpdateApplicationStatusDto statusDto, string ownerId)
    {

        var job = await _jobRepository.GetJobByIdAsync(jobId);

        if (job == null && job.UserId != ownerId)
        {
            return false;
        }

        var existApplication = job.Applications.Where(_application => _application.Id == applicationId).First();
        if (existApplication == null)
        {
            return false;
        }

        existApplication.Status = statusDto.Status;
        existApplication.Notes = statusDto.Notes;


        var emailNotification = new EmailNotification
        {
            JobId = jobId,
            Username = existApplication.Username,
            Email = existApplication.Email,
            ScheduledDate = existApplication.InterviewDate.Value.Date,
            Type = "statusUpdate",
            Message = ""
        };

        if (existApplication.Status == "Interview")
        {

            existApplication.InterviewDate = statusDto.DateToRemeber;

            emailNotification.Message = $"Dear {existApplication.Username},\n\n" +
            $"We are pleased to inform you that your interview for the position of '{job.Title}' at '{job.Company}' is scheduled for {existApplication.InterviewDate.Value:MMMM dd, yyyy} at {existApplication.InterviewDate.Value:hh:mm tt}.\n\n" +
            "Please make sure to be prepared and arrive on time. If you have any questions or require further assistance, don't hesitate to reach out to us.\n\n" +
            "Best regards,\nJobTrackerApp Team";

            var goodLuckNotification = GenerateGoodLuckMessageAsync(jobId, existApplication, job.Title, job.Company);
            _kafkaProducer.Produce("job-topic", Guid.NewGuid().ToString(), goodLuckNotification);
        }

        if (existApplication.Status == "Rejected")
        {
            emailNotification.Message = $"Dear {existApplication.Username},\n\n" +
            $"We regret to inform you that you have not been selected for the position of '{job.Title}' at '{job.Company}'.\n\n" +
            "We appreciate the time and effort you spent in the application process and wish you the best of luck in your future endeavors.\n\n" +
            "Best regards,\nJobTrackerApp Team";
        }

        if (existApplication.Status == "Selected")
        {
            emailNotification.Message = $"Dear {existApplication.Username},\n\n" +
            $"Congratulations! We are excited to inform you that you have been selected for the position of '{job.Title}' at '{job.Company}'.\n\n" +
            $"Your official start date is {statusDto.DateToRemeber:MMMM dd, yyyy}.\n\n" +
            "Our team will reach out to you shortly with further instructions regarding the next steps. We are excited to have you on board!\n\n" +
            "Best regards,\nJobTrackerApp Team";
        }
        await GenerateKafkaMessageAsync(emailNotification);

        await _jobRepository.UpdateJobAsync(job);
        return true;
    }

    private async Task GenerateKafkaMessageAsync(EmailNotification emailNotificaiton)
    {
        _kafkaProducer.Produce("job-topic", Guid.NewGuid().ToString(), emailNotificaiton);
    }

    private async Task<EmailNotification> GenerateGoodLuckMessageAsync(string jobId, Application application, string title, string company)
    {
        var goodLuckNotification = new EmailNotification
        {
            JobId = jobId,
            Username = application.Username,
            Email = application.Email,
            ScheduledDate = application.InterviewDate.Value,
            Type = "goodLuck",
            Message = $"Dear {application.Username},\n\n" +
                    $"Good Luck on your interview for '{title}' at '{company}'!\n\n" +
                    "We hope you have a great experience. Please make sure to be prepared and arrive on time. If you have any questions or require further assistance, don't hesitate to reach out to us.\n\n" +
                    "Best regards,\nJobTrackerApp Team"
        };
        _kafkaProducer.Produce("job-topic", Guid.NewGuid().ToString(), goodLuckNotification);
        return goodLuckNotification;
    }
}