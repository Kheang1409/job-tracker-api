namespace JobTracker.NotificationService.Domain.Entities;

public class Selected : EmailBase
{
    public string Title { get; private set; } = string.Empty;
    public string CompanyName { get; private set; } = string.Empty;

    private Selected(string recipient,
        string subject,
        string firstname,
        string title,
        string companyName)
        : base(recipient, subject, firstname)
    {
        Title = title;
        CompanyName = companyName;
    }

    public static Selected Create(
        string recipient,
        string subject,
        string firstname,
        string title,
        string companyName)
    {
        return new Selected(recipient, subject, firstname, title, companyName);
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
                    .footer {{
                        font-size: 12px;
                        color: #999;
                        text-align: center;
                        margin-top: 40px;
                    }}
                    .job-title {{
                        font-weight: bold;
                        color: #004aad;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='logo'>
                        <img src='https://kheang1409.github.io/jobtracker-assets/logo.png' alt='JobTracker Logo' width='120'/>
                    </div>
                    <p>Dear {FirstName},</p>
                    <p>We are thrilled to inform you that you have been <strong>selected</strong> for the position of 
                    <span class='job-title'>{Title}</span> at <strong>{CompanyName}</strong>!</p>
                    <p>Your experience and qualifications stood out among the many applicants, and we are excited about the potential you bring to our team.</p>
                    <p>Our HR department will reach out to you shortly with details regarding the next steps in the onboarding process, including any necessary documentation and start dates.</p>
                    <p>Welcome aboard — we look forward to working with you and supporting your journey at <strong>{CompanyName}</strong>.</p>
                    <p>Warm regards,<br/>The JobTracker Team</p>
                    <div class='footer'>
                        &copy; {(DateTime.UtcNow.Year)} JobTracker. All rights reserved.
                    </div>
                </div>
            </body>
        </html>";
    }
}