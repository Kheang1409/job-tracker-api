using JobTracker.JobApplicationService.Application.Repositories;
using JobTracker.JobApplicationService.Domain.Entities;
using MediatR;

namespace JobTracker.JobApplicationService.Application.JobApplications;

public sealed class GetJobApplicationsHandler(IJobApplicationRepository repository)
    : IRequestHandler<GetJobApplicationsQuery, IReadOnlyList<JobApplication>>
{
    public Task<IReadOnlyList<JobApplication>> Handle(GetJobApplicationsQuery request, CancellationToken cancellationToken) =>
        repository.GetByUserIdAsync(request.UserId);
}
