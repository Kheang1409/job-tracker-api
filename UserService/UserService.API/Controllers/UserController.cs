using JobTracker.UserService.Application.Users.Queries.GetUserProfile;
using JobTracker.UserService.Application.Users.Queries.GetUsers;
using JobTracker.UserService.Application.Users.Commands.CreateUser;
using JobTracker.UserService.Application.Users.Commands.DeleteUser;
using JobTracker.UserService.Application.Users.Commands.UpdateUser;
using JobTracker.UserService.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediatR;

namespace JobTracker.UserService.API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] CreateUserCommand command)
    {
        if (command == null)
            return BadRequest("Invalid input data.");
        var userId = await _mediator.Send(command);
        return CreatedAtAction(nameof(Profile), new { id = userId }, command);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var user = await _mediator.Send(new GetUserProfileQuery(userId));
        return Ok((UserDetailDto)user);
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var commandWithId = new UpdateUserWithIdCommand(
            userId,
            command.FirstName,
            command.LastName,
            command.Bio,
            command.Gender,
            command.Email,
            command.CountryCode,
            command.PhoneNumber);
        await _mediator.Send(commandWithId);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("me")]
    public async Task<IActionResult> Delete()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        await _mediator.Send(new DeleteUserCommand(userId));
        return NoContent();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query)
    {
        var users = await _mediator.Send(query);
        return Ok(users.Select(u => (UserDto)u));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Profile(string id)
    {
        var user = await _mediator.Send(new GetUserProfileQuery(id));
        return Ok((UserDetailDto)user);
    }
}