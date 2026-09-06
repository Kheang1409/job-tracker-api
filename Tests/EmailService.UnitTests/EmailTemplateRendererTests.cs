using JobTracker.EmailService;

namespace JobTracker.EmailService.UnitTests;

public sealed class EmailTemplateRendererTests
{
    private readonly EmailTemplateRenderer _renderer = new();

    [Fact]
    public void Verification_RendersEncodedLinkAndHtmlSafeName()
    {
        var request = new EmailRequest("verification", "user+tag@example.com", "<Admin>", "a/b+c");

        var result = _renderer.Render(request, "https://jobs.example/");

        Assert.Equal("Verify your JobTracker email", result.Subject);
        Assert.Contains("Hi &lt;Admin&gt;", result.HtmlBody);
        Assert.Contains("email=user%2Btag%40example.com", result.HtmlBody);
        Assert.Contains("token=a%2Fb%2Bc", result.HtmlBody);
    }

    [Fact]
    public void PasswordReset_RendersOtpLink()
    {
        var result = _renderer.Render(
            new EmailRequest("password-reset", "user@example.com", "Test", "123456"),
            "https://jobs.example");

        Assert.Equal("Reset your JobTracker password", result.Subject);
        Assert.Contains("/reset-password?email=user%40example.com&amp;otp=123456", result.HtmlBody.Replace("&otp", "&amp;otp"));
    }

    [Fact]
    public void UnknownType_IsRejected() =>
        Assert.Throws<ArgumentException>(() => _renderer.Render(
            new EmailRequest("unknown", "user@example.com", "Test", "token"),
            "https://jobs.example"));
}
