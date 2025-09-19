using JobTracker.JobService.Application.JobLocations.Commands.DeletePost;
using JobTracker.JobService.Application.JobLocations.Commands.UpdatePostStatus;
using JobTracker.JobService.Application.JobLocations.Commands.UpdatePost;
using JobTracker.JobService.Application.JobLocations.Commands.CreatePost;
using JobTracker.JobService.Application.JobLocations.Queries.GetPosts;
using JobTracker.JobService.Application.JobLocations.Queries.GetPost;
using JobTracker.JobService.Application.JobLocations.Queries.GetPostCount;
using JobTracker.JobService.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediatR;

namespace JobService.Controllers;

[Authorize]
[ApiController]
[Route("api/jobs")]
public class JobController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("total")]
    public async Task<IActionResult> GetPosts([FromQuery] GetPostCountQuery query)
    {   
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var queryWithId = new GetPostCountQueryWithId(
            query.Title,
            query.CompanyName,
            ownerId,
            query.PageNumber,
            query.Limit
        );
        var count = await _mediator.Send(queryWithId);
        return Ok(count);
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts([FromQuery] GetPostsQuery query)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var queryWithId = new GetPostsQueryWithId(
            query.Title,
            query.CompanyName,
            ownerId,
            query.PageNumber,
            query.Limit
        );
        var posts = await _mediator.Send(queryWithId);
        return Ok(posts.Select(j => (PostDto)j));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(string id)
    {
        var post = await _mediator.Send(new GetPostQuery(id));
        return Ok((PostDetailDto)post);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand command)
    {
        if (command == null)
            return BadRequest("Invalid input data.");
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var commandWithId = new CreatePostWithIdCommand(
            userId,
            command.Title,
            command.CompanyName,
            command.WorkMode,
            command.EmploymentType,
            command.NumberOfOpenings,
            command.MinExperience,
            command.MaxExperience,
            command.MinSalary,
            command.MaxSalary,
            command.Currency,
            command.Skills,
            command.Description,
            command.Address,
            command.ExpirationDate
        );
        var postId = await _mediator.Send(commandWithId);
        return CreatedAtAction(nameof(GetPostById), new { id = postId }, commandWithId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(string id, [FromBody] UpdatePostCommand command)
    {
        if (command == null)
            return BadRequest("Invalid input data.");
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var commandWithId = new UpdatePostWithIdCommand(
            userId,
            id,
            command.Title,
            command.CompanyName,
            command.WorkMode,
            command.EmploymentType,
            command.NumberOfOpenings,
            command.MinExperience,
            command.MaxExperience,
            command.MinSalary,
            command.MaxSalary,
            command.Currency,
            command.Skills,
            command.Description,
            command.Address,
            command.ExpirationDate,
            command.Status
        );
        await _mediator.Send(commandWithId);
        return NoContent();
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdatePostStatus(string id, [FromBody] UpdateStatusPostCommand command)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var commandId = new UpdatePostStatusWithIdCommand(
            userId,
            id,
            command.Status
        );
        await _mediator.Send(commandId);
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        await _mediator.Send(new DeletePostCommand(userId, id));
        return NoContent();
    }
}