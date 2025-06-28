namespace JobTracker.NotificationService.Domain.Entities;

public abstract class EmailBase
{
    public string Recipient { get; private set; } = string.Empty;
    public string Subject { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;

    protected EmailBase(
        string recipient,
        string subject,
        string firstName
    )
    {
        Recipient = recipient;
        Subject = subject;
        FirstName = firstName;
    }

    public abstract string Message();

}