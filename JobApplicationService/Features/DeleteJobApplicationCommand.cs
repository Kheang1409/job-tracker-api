using MediatR;

namespace JobTracker.JobApplicationService.Application.JobApplications;

public sealed record DeleteJobApplicationCommand(string UserId, string Id) : IRequest<bool>;
