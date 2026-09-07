using JobTracker.JobApplicationService.Domain.Entities;
using MediatR;

namespace JobTracker.JobApplicationService.Application.JobApplications;

public sealed record UpdateJobApplicationCommand(
    string UserId, string Id, string Company, string Role, string Source, string Status,
    DateTime AppliedOn, string Url, string Notes, DateTime? ReminderAt) : IRequest<JobApplication>;
