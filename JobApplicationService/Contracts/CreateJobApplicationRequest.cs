namespace JobTracker.JobApplicationService.Contracts.JobApplications;

public sealed record CreateJobApplicationRequest(
    string Company,
    string Role,
    string Source,
    string Status,
    DateTime AppliedOn,
    string Url,
    string Notes,
    DateTime? ReminderAt);
