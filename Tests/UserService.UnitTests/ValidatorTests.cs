using JobTracker.UserService.Application.Auths.Commands.ResetPassword;
using JobTracker.UserService.Application.Users.Commands.CreateUser;

namespace JobTracker.UserService.UnitTests;

public sealed class ValidatorTests
{
    [Fact]
    public void CreateUserValidator_AcceptsStrongValidRequest()
    {
        var result = new CreateUserCommandValidator().Validate(
            new CreateUserCommand("tester", "Test", "User", "user@example.com", "SecurePass1"));
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("12345a")]
    [InlineData("1234567")]
    public void ResetPasswordValidator_RejectsInvalidOtp(string otp)
    {
        var result = new ResetPasswordCommandValidator().Validate(
            new ResetPasswordCommand("user@example.com", otp, "SecurePass1"));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ResetPasswordCommand.OTP));
    }
}
