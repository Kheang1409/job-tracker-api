using JobTracker.UserService.API.Contracts.Auth;
using JobTracker.UserService.Application.Auths.Commands.ForgotPassword;
using JobTracker.UserService.Application.Auths.Commands.Login;
using JobTracker.UserService.Application.Auths.Commands.ResetPassword;
using JobTracker.UserService.Application.Auths.Commands.VerifyEmail;
using MediatR;

namespace JobTracker.UserService.API.Endpoints;

public static class AuthenticationEndpoints
{
    public static IEndpointRouteBuilder MapAuthenticationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var auth = endpoints.MapGroup("/api/auth");
        auth.MapPost("/forgot-password", ForgotPassword).RequireRateLimiting("account-recovery");
        auth.MapPost("/reset-password", ResetPassword).RequireRateLimiting("account-recovery");
        auth.MapPost("/login", Login).RequireRateLimiting("authentication");
        auth.MapPost("/verify-email", VerifyEmail).RequireRateLimiting("account-recovery");
        return endpoints;
    }

    private static async Task<IResult> ForgotPassword(ForgotPasswordCommand command, IMediator mediator)
    {
        await mediator.Send(command);
        return Results.Ok(new MessageResponse("If an account exists, a password reset link has been sent."));
    }

    private static async Task<IResult> ResetPassword(ResetPasswordCommand command, IMediator mediator)
    {
        await mediator.Send(command);
        return Results.NoContent();
    }

    private static async Task<IResult> Login(LoginCommand command, IMediator mediator)
    {
        var token = await mediator.Send(command);
        return Results.Ok(new TokenResponse(token));
    }

    private static async Task<IResult> VerifyEmail(VerifyEmailCommand command, IMediator mediator)
    {
        await mediator.Send(command);
        return Results.NoContent();
    }
}
