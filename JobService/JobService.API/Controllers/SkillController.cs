using JobTracker.JobService.Application.Skills.Commands.CreateSkill;
using JobTracker.JobService.Application.Skills.Commands.DeleteSkill;
using JobTracker.JobService.Application.Skills.Queries.GetSkill;
using JobTracker.JobService.Application.Skills.Queries.GetSkills;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediatR;

namespace JobTracker.JobService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/jobs/{jobId}/skills")]
public class SkillController : ControllerBase
{
    private readonly IMediator _mediator;

    public SkillController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetSkills(string jobId)
    {
        var skils = await _mediator.Send(new GetSkillsQuery(jobId));
        return Ok(skils);
    }

    [AllowAnonymous]
    [HttpGet("{skillId}")]
    public async Task<IActionResult> GetSkillById(string jobId, string skillId)
    {
        var user = await _mediator.Send(new GetSkillQuery(jobId, skillId));
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> AddSkill(string jobId, [FromBody] CreateSkillCommand command)
    {
        if (command == null)
            return BadRequest("Invalid input data.");
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var skillId = await _mediator.Send(new CreateSkillWithIdCommand(userId, jobId, command.Name));
        return CreatedAtAction(nameof(GetSkillById), new { jobId = jobId, SkillId = skillId,  }, command);
    }

    [HttpDelete("{skillId}")]
    public async Task<IActionResult> DeleteSkill(string jobId, string skillId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        await _mediator.Send(new DeleteSkillCommand(userId, jobId, skillId));
        return NoContent();
    }
}