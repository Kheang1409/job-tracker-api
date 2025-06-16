namespace JobTracker.NotificationService.Domain.Entities;

public class Applied : EmailBase
{
    public string Title { get; private set; } = string.Empty;
    public string CompanyName { get; private set; } = string.Empty;

    private Applied(string recipient,
        string subject,
        string firstname,
        string title,
        string companyName)
        : base(recipient, subject, firstname)
    {
        Title = title;
        CompanyName = companyName;
    }

    public static Applied Create(
        string recipient,
        string subject,
        string firstname,
        string title,
        string companyName)
    {
        return new Applied(recipient, subject, firstname, title, companyName);
    }
    public override string Message()
    {
        return @$"Dear {Firstname},\n\n
        Thank you for applying for the position of '{Title}' at {CompanyName}.\n\n
        We have received your application and our team is currently reviewing your qualifications.
        If your profile matches our requirements, we will contact you for the next steps in the process.\n\n
        We appreciate your interest in joining our team.\n\n
        Best regards,\n
        The JobTracker Team";
    }
}