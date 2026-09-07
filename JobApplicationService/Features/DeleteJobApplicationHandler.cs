using JobTracker.JobApplicationService.Application.Repositories;
using MediatR;

namespace JobTracker.JobApplicationService.Application.JobApplications;

public sealed class DeleteJobApplicationHandler(IJobApplicationRepository repository)
    : IRequestHandler<DeleteJobApplicationCommand, bool>
{
    public Task<bool> Handle(DeleteJobApplicationCommand request, CancellationToken cancellationToken) =>
        repository.DeleteAsync(request.UserId, request.Id);
}
