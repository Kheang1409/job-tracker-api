using JobTracker.UserService.API.Contracts.Users;
using JobTracker.UserService.Application.DTOs;
using JobTracker.UserService.Application.Users.Commands.CreateUser;
using JobTracker.UserService.Application.Users.Commands.DeleteUser;
using JobTracker.UserService.Application.Users.Commands.UpdateUser;
using JobTracker.UserService.Application.Users.Queries.GetUserProfile;
using MediatR;

namespace JobTracker.UserService.API.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var users = endpoints.MapGroup("/api/users");
        users.MapPost("", CreateUser).RequireRateLimiting("account-recovery");
        users.MapGet("/me", GetCurrentUser).RequireAuthorization();
        users.MapPut("/me", UpdateCurrentUser).RequireAuthorization();
        users.MapDelete("/me", DeleteCurrentUser).RequireAuthorization();
        return endpoints;
    }

    private static async Task<IResult> CreateUser(CreateUserCommand command, IMediator mediator)
    {
        var userId = await mediator.Send(command);
        return Results.Created($"/api/users/{userId}", new CreateUserResponse(userId));
    }

    private static async Task<IResult> GetCurrentUser(HttpContext context, IMediator mediator)
    {
        var user = await mediator.Send(new GetUserProfileQuery(context.GetUserId()));
        return Results.Ok((UserDetailDto)user);
    }

    private static async Task<IResult> UpdateCurrentUser(UpdateUserCommand command, HttpContext context, IMediator mediator)
    {
        var request = new UpdateUserWithIdCommand(
            context.GetUserId(), command.FirstName, command.LastName, command.Email,
            command.ContactNumber, command.ContactCountry, command.Bio, command.Skills,
            command.Experiences, command.Projects, command.Address);
        await mediator.Send(request);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteCurrentUser(HttpContext context, IMediator mediator)
    {
        await mediator.Send(new DeleteUserCommand(context.GetUserId()));
        return Results.NoContent();
    }
}
