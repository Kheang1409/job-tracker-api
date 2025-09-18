using JobTracker.UserService.Application.Auths.Commands.ForgotPassword;
using JobTracker.UserService.Application.Auths.Commands.ResetPassword;
using JobTracker.UserService.Application.Auths.Commands.Login;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        var token =  await _mediator.Send(command);
        return Ok(new { Token = token });
    }
    [Authorize]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var commandWithId = new ResetPasswordWithIdCommand(
            userId,
            command.OTP,
            command.Password
        );
        await _mediator.Send(commandWithId);
        return NoContent();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var token = await _mediator.Send(command);
        return Ok(new { Token = token });
    }
}