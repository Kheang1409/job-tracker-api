namespace JobTracker.NotificationService.Domain.Entities;

public class ResetPassword : EmailBase
{
    public string OTP { get; private set; } = string.Empty;

    private ResetPassword(string recipient,
        string subject,
        string firstname,
        string otp)
        : base(recipient, subject, firstname)
    {
        OTP = otp;
    }

    public static ResetPassword Create(
        string recipient,
        string subject,
        string firstname,
        string otp)
    {
        return new ResetPassword(recipient, subject, firstname, otp);
    }
    public override string Message()
    {
        return $"Dear {Firstname},\n\n" +
           "We received a request to reset your password. " +
           "Please use the following One-Time Password (OTP) to proceed:\n\n" +
           $"OTP: {OTP}\n\n" +
           "If you did not request this, please ignore this email.\n\n" +
           "Thank you,\n" +
           "The JobTracker Team";
    }
}