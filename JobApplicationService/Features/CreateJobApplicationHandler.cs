using JobTracker.JobApplicationService.Application.Repositories;
using JobTracker.JobApplicationService.Domain.Entities;
using MediatR;

namespace JobTracker.JobApplicationService.Application.JobApplications;

public sealed class CreateJobApplicationHandler(IJobApplicationRepository repository)
    : IRequestHandler<CreateJobApplicationCommand, JobApplication>
{
    public async Task<JobApplication> Handle(CreateJobApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = new JobApplication(
            request.UserId, request.Company, request.Role, request.Source, request.Status,
            request.AppliedOn, request.Url, request.Notes, request.ReminderAt);
        await repository.AddAsync(application);
        return application;
    }
}
