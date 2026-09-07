using System.Net;

namespace JobTracker.EmailService;

public sealed class EmailTemplateRenderer
{
    public (string Subject, string HtmlBody) Render(EmailRequest request, string frontendOrigin)
    {
        ArgumentNullException.ThrowIfNull(request);
        var firstName = WebUtility.HtmlEncode(request.FirstName);
        var origin = frontendOrigin.TrimEnd('/');
        var email = Uri.EscapeDataString(request.Recipient);
        var token = Uri.EscapeDataString(request.Token);

        return request.Type switch
        {
            "verification" => (
                "Verify your JobTracker email",
                BuildTemplate(
                    firstName,
                    "Welcome to JobTracker",
                    "Verify your email to start organizing your job search.",
                    "Your career command center",
                    "Keep every opportunity moving forward in one calm place. Track applications from LinkedIn, Indeed, referrals, or direct applications, then manage statuses, notes, reminders, and follow-ups without losing momentum.",
                    "Verify my email",
                    $"{origin}/verify-email?email={email}&token={token}",
                    "This secure verification link expires in 24 hours.")),
            "password-reset" => (
                "Reset your JobTracker password",
                BuildTemplate(
                    firstName,
                    "Reset your password",
                    "Let’s get you safely back to your job-search dashboard.",
                    "Your applications are waiting",
                    "Return to your organized view of every opportunity, including application statuses, notes, reminders, interviews, and next steps.",
                    "Reset my password",
                    $"{origin}/reset-password?email={email}&otp={token}",
                    "For your security, this reset link expires in 3 minutes. If you didn’t request it, you can safely ignore this email.")),
            _ => throw new ArgumentException($"Unsupported email type '{request.Type}'.", nameof(request))
        };
    }

    private static string BuildTemplate(
        string firstName,
        string heading,
        string introduction,
        string featureHeading,
        string featureDescription,
        string buttonLabel,
        string actionUrl,
        string securityNotice)
    {
        var safeUrl = WebUtility.HtmlEncode(actionUrl);

        return $$"""
            <!doctype html>
            <html lang="en">
            <head>
              <meta charset="utf-8">
              <meta name="viewport" content="width=device-width, initial-scale=1">
              <title>{{heading}}</title>
            </head>
            <body style="margin:0;background:#f7f8fc;color:#0f172a;font-family:Inter,-apple-system,BlinkMacSystemFont,'Segoe UI',sans-serif;">
              <div style="display:none;max-height:0;overflow:hidden;opacity:0;">{{introduction}}</div>
              <table role="presentation" width="100%" cellspacing="0" cellpadding="0" border="0" style="background:#f7f8fc;">
                <tr>
                  <td align="center" style="padding:32px 16px;">
                    <table role="presentation" width="100%" cellspacing="0" cellpadding="0" border="0" style="max-width:600px;">
                      <tr>
                        <td style="padding:0 4px 18px;font-size:20px;font-weight:800;letter-spacing:-0.4px;color:#0f172a;">
                          <span style="display:inline-block;width:30px;height:30px;line-height:30px;margin-right:9px;border-radius:9px;background:#4f46e5;color:#ffffff;text-align:center;font-size:15px;">JT</span>
                          JobTracker
                        </td>
                      </tr>
                      <tr>
                        <td style="overflow:hidden;border:1px solid #e2e8f0;border-radius:22px;background:#ffffff;box-shadow:0 8px 30px rgba(15,23,42,0.06);">
                          <div style="padding:38px 36px;background:#0f172a;background-image:linear-gradient(135deg,#0f172a 0%,#312e81 100%);color:#ffffff;">
                            <p style="margin:0 0 12px;color:#c7d2fe;font-size:11px;font-weight:800;letter-spacing:2px;text-transform:uppercase;">Job search, organized</p>
                            <h1 style="margin:0;font-size:30px;line-height:1.2;letter-spacing:-0.7px;">{{heading}}</h1>
                            <p style="margin:14px 0 0;color:#e0e7ff;font-size:16px;line-height:1.65;">{{introduction}}</p>
                          </div>
                          <div style="padding:34px 36px 38px;">
                            <p style="margin:0 0 12px;font-size:16px;line-height:1.6;">Hi {{firstName}},</p>
                            <p style="margin:0 0 24px;color:#475569;font-size:15px;line-height:1.7;">{{featureDescription}}</p>
                            <div style="margin:0 0 28px;padding:18px;border:1px solid #e0e7ff;border-radius:14px;background:#eef2ff;">
                              <p style="margin:0 0 6px;color:#4338ca;font-size:12px;font-weight:800;letter-spacing:1px;text-transform:uppercase;">{{featureHeading}}</p>
                              <p style="margin:0;color:#3730a3;font-size:14px;line-height:1.6;">One dashboard for applications, progress, interviews, notes, and the actions that deserve your attention.</p>
                            </div>
                            <table role="presentation" cellspacing="0" cellpadding="0" border="0">
                              <tr>
                                <td style="border-radius:12px;background:#4f46e5;">
                                  <a href="{{safeUrl}}" style="display:inline-block;padding:14px 22px;color:#ffffff;font-size:15px;font-weight:800;text-decoration:none;">{{buttonLabel}}</a>
                                </td>
                              </tr>
                            </table>
                            <p style="margin:22px 0 0;color:#64748b;font-size:13px;line-height:1.6;">{{securityNotice}}</p>
                            <p style="margin:24px 0 0;padding-top:20px;border-top:1px solid #e2e8f0;color:#94a3b8;font-size:12px;line-height:1.6;word-break:break-all;">Button not working? Copy and paste this link:<br><a href="{{safeUrl}}" style="color:#4f46e5;text-decoration:underline;">{{safeUrl}}</a></p>
                          </div>
                        </td>
                      </tr>
                      <tr>
                        <td align="center" style="padding:20px 16px 0;color:#94a3b8;font-size:12px;line-height:1.6;">JobTracker · Keep every opportunity moving forward.</td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>
            </body>
            </html>
            """;
    }
}
