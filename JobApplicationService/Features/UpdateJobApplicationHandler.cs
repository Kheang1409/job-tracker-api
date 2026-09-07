using JobTracker.SharedKernel.Exceptions;
using JobTracker.JobApplicationService.Application.Repositories;
using JobTracker.JobApplicationService.Domain.Entities;
using MediatR;

namespace JobTracker.JobApplicationService.Application.JobApplications;

public sealed class UpdateJobApplicationHandler(IJobApplicationRepository repository)
    : IRequestHandler<UpdateJobApplicationCommand, JobApplication>
{
    public async Task<JobApplication> Handle(UpdateJobApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await repository.GetByIdAsync(request.UserId, request.Id)
            ?? throw new NotFoundException("The job application was not found.");
        application.UpdateDetails(request.Company, request.Role, request.Source, request.AppliedOn,
            request.Url, request.Notes, request.ReminderAt);
        application.ChangeStatus(request.Status);
        await repository.UpdateAsync(application);
        return application;
    }
}
