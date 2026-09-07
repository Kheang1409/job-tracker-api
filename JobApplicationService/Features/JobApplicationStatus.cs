namespace JobTracker.JobApplicationService.Application.JobApplications;

internal static class JobApplicationStatus
{
    private static readonly string[] Values = ["Saved", "Applied", "Interview", "Offer", "Rejected"];

    internal static bool IsValid(string status) =>
        Values.Contains(status, StringComparer.OrdinalIgnoreCase);
}
