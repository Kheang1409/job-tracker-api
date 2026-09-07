using JobTracker.JobApplicationService.Domain.Entities;
using MediatR;

namespace JobTracker.JobApplicationService.Application.JobApplications;

public sealed record GetJobApplicationsQuery(string UserId) : IRequest<IReadOnlyList<JobApplication>>;
