using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Entities;
using MediatR;
using JobTracker.SharedKernel.Exceptions;

namespace JobTracker.UserService.Application.JobApplications;

public sealed record GetJobApplicationsQuery(string UserId) : IRequest<IReadOnlyList<JobApplication>>;

public sealed record CreateJobApplicationCommand(
    string UserId, string Company, string Role, string Source, string Status,
    DateTime AppliedOn, string Url, string Notes, DateTime? ReminderAt) : IRequest<JobApplication>;

public sealed record UpdateJobApplicationCommand(
    string UserId, string Id, string Company, string Role, string Source, string Status,
    DateTime AppliedOn, string Url, string Notes, DateTime? ReminderAt) : IRequest<JobApplication>;

public sealed record DeleteJobApplicationCommand(string UserId, string Id) : IRequest<bool>;

public sealed class GetJobApplicationsHandler(IJobApplicationRepository repository)
    : IRequestHandler<GetJobApplicationsQuery, IReadOnlyList<JobApplication>>
{
    public Task<IReadOnlyList<JobApplication>> Handle(GetJobApplicationsQuery request, CancellationToken cancellationToken) =>
        repository.GetByUserIdAsync(request.UserId);
}

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

public sealed class DeleteJobApplicationHandler(IJobApplicationRepository repository)
    : IRequestHandler<DeleteJobApplicationCommand, bool>
{
    public Task<bool> Handle(DeleteJobApplicationCommand request, CancellationToken cancellationToken) =>
        repository.DeleteAsync(request.UserId, request.Id);
}
