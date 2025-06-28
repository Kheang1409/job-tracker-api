namespace JobTracker.NotificationService.Domain.Entities;

public class MoveOn : EmailBase
{
    public string Title { get; private set; } = string.Empty;
    public string CompanyName { get; private set; } = string.Empty;
    public string Stage { get; private set; } = string.Empty;
    public DateTime AppointmentDate { get; private set; }
    private MoveOn(string recipient,
        string subject,
        string firstname,
        string title,
        string companyName,
        string stage,
        DateTime appointmentDate)
        : base(recipient, subject, firstname)
    {
        Title = title;
        CompanyName = companyName;
        Stage = stage;
        AppointmentDate = appointmentDate;
    }

    public static MoveOn Create(
        string recipient,
        string subject,
        string firstname,
        string title,
        string companyName,
        string stage,
        DateTime appointmentDate)
    {
        return new MoveOn(recipient, subject, firstname, title, companyName, stage, appointmentDate);
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
                    .highlight {{
                        font-weight: bold;
                        color: #007b00;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='logo'>
                        <img src='https://kheang1409.github.io/jobtracker-assets/logo.png' alt='JobTracker Logo' width='120'/>
                    </div>
                    <p>Dear {FirstName},</p>
                    <p>We are pleased to inform you that you have <span class='highlight'>successfully cleared</span> the previous round for the position of 
                    <span class='job-title'>{Title}</span> at <strong>{CompanyName}</strong>.</p>
                    <p>Congratulations!</p>
                    <p>You are now moving forward to the next stage of the hiring process: <strong>{Stage}</strong>.</p>
                    <p>Please be prepared and available on <strong>{AppointmentDate:dddd, MMMM dd, yyyy 'at' hh:mm tt}</strong>.</p>
                    <p>Further details will be shared with you soon. Stay sharp and good luck!</p>
                    <p>Best regards,<br/>The JobTracker Team</p>
                    <div class='footer'>
                        &copy; {(DateTime.UtcNow.Year)} JobTracker. All rights reserved.
                    </div>
                </div>
            </body>
        </html>";
    }
}