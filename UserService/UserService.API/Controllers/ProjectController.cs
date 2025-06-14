

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediatR;

using JobTracker.UserService.Application.Projects.Queries.GetProjects;
using JobTracker.UserService.Application.Projects.Queries.GetProject;
using JobTracker.UserService.Application.Projects.Commands.CreateProject;
using JobTracker.UserService.Application.Projects.Commands.UpdateProject;
using JobTracker.UserService.Application.Projects.Commands.DeleteProject;

namespace JobTracker.UserService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/users/me/projects")]
public class ProjectController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var skils = await _mediator.Send(new GetProjectsQuery(userId));
        return Ok(skils);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectById(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var user = await _mediator.Send(new GetProjectQuery(userId, id));
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> AddProject([FromBody] CreateProjectCommand command)
    {
        if (command == null)
            return BadRequest("Invalid input data.");
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var commandWithId = new CreateProjectWithIdCommand(
            userId,
            command.Name,
            command.About,
            command.StartDate,
            command.EndDate
        );
        var addressId = await _mediator.Send(commandWithId);
        return CreatedAtAction(nameof(GetProjectById), new { id = addressId }, commandWithId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(string id, [FromBody] UpdateProjectCommand command)
    {
        if (command == null)
            return BadRequest("Invalid input data.");
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var commandWithId = new UpdateProjectWithIdCommand(
            userId,
            id,
            command.Name,
            command.About,
            command.StartDate,
            command.EndDate);
            
        await _mediator.Send(commandWithId);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        await _mediator.Send(new DeleteProjectCommand(userId, id));
        return NoContent();
    }
}