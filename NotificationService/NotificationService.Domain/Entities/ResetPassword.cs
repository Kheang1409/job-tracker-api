namespace JobTracker.NotificationService.Domain.Entities;

public class ResetPassword : EmailBase
{
    public string OTP { get; private set; } = string.Empty;

    private ResetPassword(string recipient,
        string subject,
        string firstName,
        string otp)
        : base(recipient, subject, firstName)
    {
        OTP = otp;
    }

    public static ResetPassword Create(
        string recipient,
        string subject,
        string firstName,
        string otp)
    {
        return new ResetPassword(recipient, subject, firstName, otp);
    }
    public override string Message()
    {
        return $@"
        <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        padding: 20px;
                        color: #333;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: auto;
                        background: #ffffff;
                        padding: 30px;
                        border-radius: 8px;
                        box-shadow: 0 0 10px rgba(0,0,0,0.1);
                    }}
                    .logo {{
                        text-align: center;
                        margin-bottom: 20px;
                    }}
                    .otp {{
                        font-size: 24px;
                        font-weight: bold;
                        color: #004aad;
                        background-color: #eef3fc;
                        padding: 10px 20px;
                        display: inline-block;
                        border-radius: 6px;
                        margin: 20px 0;
                    }}
                    .footer {{
                        font-size: 12px;
                        color: #999;
                        text-align: center;
                        margin-top: 40px;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='logo'>
                        <img src='https://kheang1409.github.io/jobtracker-assets/logo.png' alt='JobTracker Logo' width='120'/>
                    </div>
                    <p>Dear {FirstName},</p>
                    <p>We received a request to reset your password. Please use the following One-Time Password (OTP) to proceed:</p>
                    <div class='otp'>{OTP}</div>
                    <p>If you did not request this, please ignore this email.</p>
                    <p>Thank you,<br/>The JobTracker Team</p>
                    <div class='footer'>
                        &copy; {(DateTime.UtcNow.Year)} JobTracker. All rights reserved.
                    </div>
                </div>
            </body>
        </html>";
    }
}