using JobTracker.UserService.Domain.Entities;

namespace JobTracker.UserService.UnitTests;

public sealed class UserTests
{
    [Fact]
    public void Build_NormalizesIdentityAndCreatesSecureCredentials()
    {
        var user = CreateUser(email: "  USER@Example.COM ");

        Assert.Equal("user@example.com", user.Email);
        Assert.True(user.Verify("SecurePass1"));
        Assert.False(user.Verify("wrong-password"));
        Assert.StartsWith("PBKDF2-SHA256$", user.PasswordHash);
        Assert.StartsWith("SHA256$", user.EmailVerificationToken);
        Assert.Matches("^[A-F0-9]{64}$", user.EmailVerificationDeliveryToken);
        Assert.Empty(user.OTP);
    }

    [Fact]
    public void VerifyEmail_WithCurrentToken_VerifiesAndConsumesToken()
    {
        var user = CreateUser();
        var token = user.EmailVerificationDeliveryToken;

        user.VerifyEmail(token);

        Assert.True(user.IsEmailVerified);
        Assert.Empty(user.EmailVerificationToken);
        Assert.Null(user.EmailVerificationExpiresAt);
        Assert.Throws<InvalidOperationException>(() => user.VerifyEmail(token));
    }

    [Fact]
    public void ForgotAndResetPassword_RotatesOtpAndClearsResetState()
    {
        var user = CreateUser();
        var otp = user.ForgotPassword();

        Assert.Matches(@"^\d{6}$", otp);
        Assert.StartsWith("SHA256$", user.OTP);
        Assert.NotNull(user.ExpireDate);

        Assert.True(user.TryConsumePasswordResetOtp(otp));

        user.ResetPassword("NewSecurePass1");

        Assert.Empty(user.OTP);
        Assert.Null(user.ExpireDate);
        Assert.True(user.Verify("NewSecurePass1"));
        Assert.False(user.Verify("SecurePass1"));
    }

    [Fact]
    public void PasswordReset_InvalidOtp_IsLockedAfterFiveAttempts()
    {
        var user = CreateUser();
        user.ForgotPassword();

        for (var attempt = 0; attempt < 5; attempt++)
            Assert.False(user.TryConsumePasswordResetOtp("000000"));

        Assert.Empty(user.OTP);
        Assert.Null(user.ExpireDate);
        Assert.Equal(5, user.PasswordResetFailedAttempts);
    }

    [Fact]
    public void Skills_AreTrimmedAndCaseInsensitiveUnique()
    {
        var user = CreateUser();

        user.AddSkill("  C# ");
        user.AddSkill("c#");

        Assert.Single(user.Skills);
        Assert.Equal("C#", user.Skills[0]);
        Assert.True(user.RemoveSkill(" c# "));
        Assert.Empty(user.Skills);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Build_RejectsMissingRequiredNames(string? firstName)
    {
        var builder = BaseBuilder().SetFirstName(firstName!);
        Assert.ThrowsAny<ArgumentException>(() => builder.Build());
    }

    private static User CreateUser(string email = "user@example.com") =>
        BaseBuilder().SetEmail(email).Build();

    private static User.Builder BaseBuilder() => new User.Builder()
        .SetUsername("tester")
        .SetFirstName("Test")
        .SetLastName("User")
        .SetEmail("user@example.com")
        .SetPassword("SecurePass1");
}
