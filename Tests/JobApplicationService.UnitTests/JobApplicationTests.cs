using JobTracker.JobApplicationService.Domain.Entities;

namespace JobTracker.JobApplicationService.UnitTests;

public sealed class JobApplicationTests
{
    [Fact]
    public void Constructor_NormalizesFieldsAndStatus()
    {
        var application = Create(status: "interview");

        Assert.Equal("Acme", application.Company);
        Assert.Equal("Engineer", application.Role);
        Assert.Equal("Interview", application.Status);
    }

    [Fact]
    public void ChangeStatus_RejectsUnknownStatus()
    {
        var application = Create();
        Assert.Throws<ArgumentException>(() => application.ChangeStatus("Pending"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_RejectsMissingCompany(string company)
    {
        Assert.Throws<ArgumentException>(() => Create(company));
    }

    private static JobApplication Create(string company = " Acme ", string status = "Applied") =>
        new("user-1", company, " Engineer ", "LinkedIn", status, DateTime.UtcNow,
            "https://example.com", "", null);
}
