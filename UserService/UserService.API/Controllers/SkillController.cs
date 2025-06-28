using JobTracker.UserService.Application.Skills.Commands.CreateSkill;
using JobTracker.UserService.Application.Skills.Commands.DeleteSkill;
using JobTracker.UserService.Application.Skills.Commands.UpdateSkill;
using JobTracker.UserService.Application.Skills.Queries.GetSkill;
using JobTracker.UserService.Application.Skills.Queries.GetSkills;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediatR;

namespace JobTracker.UserService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/users/me/skills")]
public class SkillController : ControllerBase
{
    private readonly IMediator _mediator;

    public SkillController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetSkills()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var skils = await _mediator.Send(new GetSkillsQuery(userId));
        return Ok(skils);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSkillById(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var user = await _mediator.Send(new GetSkillQuery(userId, id));
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> AddSkill([FromBody] CreateSkillCommand command)
    {
        if (command == null)
            return BadRequest("Invalid input data.");
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var skillId = await _mediator.Send(new CreateSkillWithIdCommand(userId, command.Name));
        return CreatedAtAction(nameof(GetSkillById), new { id = skillId }, command);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSkill(string id, [FromBody] UpdateSkillCommand command)
    {
        if (command == null)
            return BadRequest("Invalid input data.");
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        await _mediator.Send(new UpdateSkillWithIdCommand(userId, id, command.Name));
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSkill(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        await _mediator.Send(new DeleteSkillCommand(userId, id));
        return NoContent();
    }
}