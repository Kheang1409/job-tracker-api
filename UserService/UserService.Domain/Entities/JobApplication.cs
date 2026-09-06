using System.Security.Cryptography;

namespace JobTracker.UserService.Domain.Entities;

public sealed class JobApplication
{
    private static readonly string[] AllowedStatuses = ["Saved", "Applied", "Interview", "Offer", "Rejected"];

    public string Id { get; private set; } = Convert.ToHexString(RandomNumberGenerator.GetBytes(12)).ToLowerInvariant();
    public string UserId { get; private set; } = string.Empty;
    public string Company { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
    public string Source { get; private set; } = string.Empty;
    public string Status { get; private set; } = "Applied";
    public DateTime AppliedOn { get; private set; }
    public string Url { get; private set; } = string.Empty;
    public string Notes { get; private set; } = string.Empty;
    public DateTime? ReminderAt { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; private set; } = DateTime.UtcNow;

    private JobApplication() { }

    public JobApplication(
        string userId,
        string company,
        string role,
        string source,
        string status,
        DateTime appliedOn,
        string url,
        string notes,
        DateTime? reminderAt)
    {
        UserId = Required(userId, nameof(userId));
        Company = Required(company, nameof(company));
        Role = Required(role, nameof(role));
        Source = source?.Trim() ?? string.Empty;
        AppliedOn = appliedOn;
        Url = url?.Trim() ?? string.Empty;
        Notes = notes?.Trim() ?? string.Empty;
        ReminderAt = reminderAt;
        ChangeStatus(status);
    }

    public void ChangeStatus(string status)
    {
        var normalized = Required(status, nameof(status));
        if (!AllowedStatuses.Contains(normalized, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException($"Status must be one of: {string.Join(", ", AllowedStatuses)}.", nameof(status));

        Status = AllowedStatuses.First(value => string.Equals(value, normalized, StringComparison.OrdinalIgnoreCase));
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string company,
        string role,
        string source,
        DateTime appliedOn,
        string url,
        string notes,
        DateTime? reminderAt)
    {
        Company = Required(company, nameof(company));
        Role = Required(role, nameof(role));
        Source = source?.Trim() ?? string.Empty;
        AppliedOn = appliedOn;
        Url = url?.Trim() ?? string.Empty;
        Notes = notes?.Trim() ?? string.Empty;
        ReminderAt = reminderAt;
        ModifiedAt = DateTime.UtcNow;
    }

    private static string Required(string value, string name) =>
        string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Value cannot be empty.", name)
            : value.Trim();
}
