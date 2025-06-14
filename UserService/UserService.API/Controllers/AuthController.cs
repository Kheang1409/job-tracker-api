using JobTracker.UserService.Application.Users.Commands.ForgotPassword;
using JobTracker.UserService.Application.Users.Commands.ResetPassword;
using JobTracker.UserService.Application.Users.Commands.Login;

using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace JobTracker.UserService.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {

        if (command == null)
            return BadRequest("Invalid input data.");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        if (command == null)
            return BadRequest("Invalid input data.");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        if (command == null)
            return BadRequest("Invalid input data.");
        var token = await _mediator.Send(command);
        return Ok(new { Token = token });
    }
}