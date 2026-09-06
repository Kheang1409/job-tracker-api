namespace JobTracker.EmailService;

public sealed record EmailRequest(string Type, string Recipient, string FirstName, string Token, string? MessageId = null);
