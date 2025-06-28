using JobTracker.JobService.Application.JobLocations.Commands.DeleteJobPost;
using JobTracker.JobService.Application.JobLocations.Commands.UpdateStatusJobPost;
using JobTracker.JobService.Application.JobLocations.Commands.UpdateJobPost;
using JobTracker.JobService.Application.JobLocations.Commands.CreateJobPost;
using JobTracker.JobService.Application.JobLocations.Queries.GetJobPosts;
using JobTracker.JobService.Application.JobLocations.Queries.GetJobPost;
using JobTracker.JobService.Application.DTOs;
using JobTracker.JobService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediatR;
using JobTracker.JobService.Application.JobLocations.Commands.UpdateJobLocation;

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

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetJobs([FromQuery] GetJobPostsQuery query)
    {
        var jobPostings = await _mediator.Send(query);
        return Ok(jobPostings.Select(j => (JobPostingDto)j));
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetJobById(string id)
    {
        var jobPosting = await _mediator.Send(new GetJobPostQuery(id));
        return Ok((JobPostingDetailDto)jobPosting);
    }

    [HttpPost]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobPostCommand command)
    {
        if (command == null)
            return BadRequest("Invalid input data.");
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var commandWithId = new CreateJobPostWithIdCommand(
            userId,
            command.Title,
            command.CompanyName,
            command.WorkMode,
            command.EmploymentType,
            command.NumberOfOpenings,
            command.MinExperience,
            command.MinSalary,
            command.MaxSalary,
            command.Currency,
            command.RequiredSkills.Select(s => Skill.Create(s.Name)).ToList(),
            command.JobDescription,
            command.Address,
            command.PostalCode,
            command.City,
            command.County,
            command.State,
            command.Country,
            command.Status
        );
        var jobPostId = await _mediator.Send(commandWithId);
        return CreatedAtAction(nameof(GetJobById), new { id = jobPostId }, commandWithId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJob(string id, [FromBody] UpdateJobPostCommand command)
    {
        if (command == null)
            return BadRequest("Invalid input data.");
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var commandWithId = new UpdateJobPostWithIdCommand(
            userId,
            id,
            command.Title,
            command.CompanyName,
            command.WorkMode,
            command.EmploymentType,
            command.NumberOfOpenings,
            command.MinExperience,
            command.JobDescription
        );
        await _mediator.Send(commandWithId);
        return NoContent();
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> CloseJobPostStatus(string id, [FromBody] UpdateStatusJobPostCommand command)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var commandId = new UpdateStatusJobPostWithIdCommand(
            userId,
            id,
            command.Status
        );
        await _mediator.Send(commandId);
        return NoContent();
    }

    [HttpPut("{id}/location")]
    public async Task<IActionResult> UpdateJobStatus(string id, [FromBody] UpdateJobLocationCommand command)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var commandId = new UpdateJobLocationWithIdCommand(
            userId,
            id,
            command.Address,
            command.PostalCode,
            command.City,
            command.County,
            command.State,
            command.Country
        );
        await _mediator.Send(commandId);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        await _mediator.Send(new DeleteJobPostCommand(userId, id));
        return NoContent();
    }
}