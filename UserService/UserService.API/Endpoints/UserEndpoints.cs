using System.Security.Claims;
using JobTracker.UserService.Application.Auths.Commands.ForgotPassword;
using JobTracker.UserService.Application.Auths.Commands.Login;
using JobTracker.UserService.Application.Auths.Commands.ResetPassword;
using JobTracker.UserService.Application.Auths.Commands.VerifyEmail;
using JobTracker.UserService.Application.DTOs;
using JobTracker.UserService.Application.Users.Commands.CreateUser;
using JobTracker.UserService.Application.Users.Commands.DeleteUser;
using JobTracker.UserService.Application.Users.Commands.UpdateUser;
using JobTracker.UserService.Application.Users.Queries.GetUserProfile;
using JobTracker.UserService.Domain.Entities;
using JobTracker.UserService.Application.JobApplications;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using JobTracker.UserService.API.Hubs;

namespace JobTracker.UserService.API.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var users = endpoints.MapGroup("/api/users");
        var auth = endpoints.MapGroup("/api/auth");
        var applications = endpoints.MapGroup("/api/job-applications").RequireAuthorization();

        applications.MapGet("", async (HttpContext context, IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetJobApplicationsQuery(GetUserId(context)))));

        applications.MapPost("", async (
            CreateJobApplicationRequest request,
            HttpContext context,
            IMediator mediator,
            IHubContext<JobApplicationsHub> hub) =>
        {
            var userId = GetUserId(context);
            var application = await mediator.Send(new CreateJobApplicationCommand(
                userId, request.Company, request.Role, request.Source, request.Status,
                request.AppliedOn, request.Url, request.Notes, request.ReminderAt));
            await hub.Clients.User(userId).SendAsync("ApplicationCreated", application);
            return Results.Created($"/api/job-applications/{application.Id}", application);
        });

        applications.MapPut("/{id}", async (
            string id,
            UpdateJobApplicationRequest request,
            HttpContext context,
            IMediator mediator,
            IHubContext<JobApplicationsHub> hub) =>
        {
            var userId = GetUserId(context);
            var application = await mediator.Send(new UpdateJobApplicationCommand(
                userId, id, request.Company, request.Role, request.Source, request.Status,
                request.AppliedOn, request.Url, request.Notes, request.ReminderAt));
            await hub.Clients.User(userId).SendAsync("ApplicationUpdated", application);
            return Results.Ok(application);
        });

        applications.MapDelete("/{id}", async (
            string id,
            HttpContext context,
            IMediator mediator,
            IHubContext<JobApplicationsHub> hub) =>
        {
            var userId = GetUserId(context);
            var deleted = await mediator.Send(new DeleteJobApplicationCommand(userId, id));
            if (deleted)
            {
                await hub.Clients.User(userId).SendAsync("ApplicationDeleted", id);
            }
            return deleted ? Results.NoContent() : Results.NotFound();
        });

        users.MapPost("", async (CreateUserCommand command, IMediator mediator) =>
        {
            var userId = await mediator.Send(command);
            return Results.Created($"/api/users/{userId}", new { Id = userId });
        }).RequireRateLimiting("account-recovery");

        users.MapGet("/me", async (HttpContext context, IMediator mediator) =>
        {
            var userId = GetUserId(context);
            var user = await mediator.Send(new GetUserProfileQuery(userId));
            return Results.Ok((UserDetailDto)user);
        }).RequireAuthorization();

        users.MapPut("/me", async (UpdateUserCommand command, HttpContext context, IMediator mediator) =>
        {
            var userId = GetUserId(context);
            var commandWithId = new UpdateUserWithIdCommand(
                userId, command.FirstName, command.LastName, command.Email, command.ContactNumber, command.ContactCountry,
                command.Bio, command.Skills, command.Experiences, command.Projects, command.Address);
            await mediator.Send(commandWithId);
            return Results.NoContent();
        }).RequireAuthorization();

        users.MapDelete("/me", async (HttpContext context, IMediator mediator) =>
        {
            await mediator.Send(new DeleteUserCommand(GetUserId(context)));
            return Results.NoContent();
        }).RequireAuthorization();

        auth.MapPost("/forgot-password", async (ForgotPasswordCommand command, IMediator mediator) =>
        {
            await mediator.Send(command);
            return Results.Ok(new { Message = "If an account exists, a password reset link has been sent." });
        }).RequireRateLimiting("account-recovery");

        auth.MapPost("/reset-password", async (ResetPasswordCommand command, HttpContext context, IMediator mediator) =>
        {
            await mediator.Send(command);
            return Results.NoContent();
        }).RequireRateLimiting("account-recovery");

        auth.MapPost("/login", async (LoginCommand command, IMediator mediator) =>
        {
            var token = await mediator.Send(command);
            return Results.Ok(new { Token = token });
        }).RequireRateLimiting("authentication");

        auth.MapPost("/verify-email", async (VerifyEmailCommand command, IMediator mediator) =>
        {
            await mediator.Send(command);
            return Results.NoContent();
        }).RequireRateLimiting("account-recovery");

        return endpoints;
    }

    private static string GetUserId(HttpContext context) =>
        context.User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException();
}

public sealed record CreateJobApplicationRequest(
    string Company,
    string Role,
    string Source,
    string Status,
    DateTime AppliedOn,
    string Url,
    string Notes,
    DateTime? ReminderAt);

public sealed record UpdateJobApplicationRequest(
    string Company,
    string Role,
    string Source,
    string Status,
    DateTime AppliedOn,
    string Url,
    string Notes,
    DateTime? ReminderAt);
