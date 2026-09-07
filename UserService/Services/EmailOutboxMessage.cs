namespace JobTracker.UserService.Application.Services;

public sealed class EmailOutboxMessage
{
    public string Id { get; init; } = Guid.NewGuid().ToString("N");
    public string Type { get; init; } = string.Empty;
    public string Recipient { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string EncryptedToken { get; set; } = string.Empty;
    public string TokenNonce { get; set; } = string.Empty;
    public string TokenTag { get; set; } = string.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime NextAttemptAt { get; set; } = DateTime.UtcNow;
    public DateTime? LeaseExpiresAt { get; set; }
    public int Attempts { get; set; }
    public string? LastError { get; set; }

    public static EmailOutboxMessage Verification(string recipient, string firstName, string token) =>
        new() { Type = "verification", Recipient = recipient, FirstName = firstName, Token = token };

    public static EmailOutboxMessage PasswordReset(string recipient, string firstName, string otp) =>
        new() { Type = "password-reset", Recipient = recipient, FirstName = firstName, Token = otp };
}
