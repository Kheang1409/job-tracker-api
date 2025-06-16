namespace JobTracker.NotificationService.Domain.Entities;

public abstract class EmailBase
{
    public string Recipient { get; private set; } = string.Empty;
    public string Subject { get; private set; } = string.Empty;
    public string Firstname { get; private set; } = string.Empty;

    protected EmailBase(
        string recipient,
        string subject,
        string firstname
    )
    {
        Recipient = recipient;
        Subject = subject;
        Firstname = firstname;
    }

    public abstract string Message();

}