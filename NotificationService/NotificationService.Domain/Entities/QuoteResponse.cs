namespace JobTracker.NotificationService.Domain.Entities;

public class QuoteResponse : EmailBase
{
    public string Quote { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;

    private QuoteResponse(string recipient, string subject, string firstName, string quote, string author, string category)
        : base(recipient, subject, firstName)
    {
        Quote = quote;
        Author = author;
        Category = category;
    }

    public static QuoteResponse Create(string recipient, string subject, string firstName, string quote, string author, string category)
    {
        return new QuoteResponse(recipient, subject, firstName, quote, author, category);
    }

    public override string Message()
    {
        return $@"
        <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f9f9f9;
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
                    .quote-box {{
                        font-size: 20px;
                        font-style: italic;
                        color: #1a73e8;
                        background-color: #eef2fb;
                        padding: 20px;
                        border-radius: 8px;
                        margin: 30px 0;
                        line-height: 1.5;
                    }}
                    .author {{
                        font-size: 16px;
                        text-align: right;
                        color: #555;
                        margin-top: -10px;
                        margin-bottom: 20px;
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
                    <p>Hi {FirstName},</p>
                    <p>Here's a little motivation for you today:</p>
                    <div class='quote-box'>“{Quote}”</div>
                    <div class='author'>– {Author}</div>
                    <p>Keep pushing forward. You've got this!</p>
                    <p>– The JobTracker Team</p>
                    <div class='footer'>
                        &copy; {(DateTime.UtcNow.Year)} JobTracker. All rights reserved.
                    </div>
                </div>
            </body>
        </html>";
    }
}
