
using JobTracker.JobService.Application.Candidates.Applied.Commands;
using JobTracker.JobService.Application.Candidates.Queries.GetCandidates;
using JobTracker.JobService.Application.Candidates.Queries.GetCandidate;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediatR;
using JobTracker.JobService.Application.DTOs;
using JobTracker.JobService.Application.Candidates.Withdraw.Commands;
using JobTracker.JobService.Application.Candidates.MoveOn.Commands;
using JobTracker.JobService.Application.Candidates.Rejected.Commands;
using JobTracker.JobService.Application.Candidates.Selected.Commands;

namespace JobService.Controllers;

[Authorize]
[ApiController]
[Route("api/jobs/{jobId}/candidates")]
public class CandidateController : ControllerBase
{
    private readonly IMediator _mediator;

    public CandidateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetCandidateById(string jobId, [FromQuery] GetCandidatesQuery query)
    {
        var queryId = new GetCandidatesWithIdQuery(
            jobId,
            query.PageNumber,
            query.Limit
        );
        var candidates = await _mediator.Send(queryId);
        return Ok(candidates.Select(c => (CandidateDto)c));
    }

    [AllowAnonymous]
    [HttpGet("{candidateId}")]
    public async Task<IActionResult> GetCandidateById(string jobId, string candidateId)
    {
        var candidate = await _mediator.Send(new GetCandidateQuery(jobId, candidateId));
        return Ok((CandidateDetailDto)candidate);
    }

    [HttpPost]
    public async Task<IActionResult> Applied(string jobId)
    {
        var candidateId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();
        var firstName = User.FindFirstValue(ClaimTypes.GivenName) ?? throw new UnauthorizedAccessException();
        var lastName = User.FindFirstValue(ClaimTypes.Surname) ?? throw new UnauthorizedAccessException();
        var command = new AppliedCommand(
            jobId,
            candidateId,
            firstName,
            lastName,
            email
        );
        var commandId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCandidateById), new { jobId, commandId }, command);
    }
    [HttpDelete("me")]
    public async Task<IActionResult> Withdraw(string jobId)
    {
        var candidateId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var command = new WithdrawCommand(
            jobId,
            candidateId
        );
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("{candidateId}")]
    public async Task<IActionResult> MoveOn(string jobId, string candidateId, [FromBody] MoveOnCommand command)
    {
        var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var commandWithId = new MoveOnWithIdCommand(
            authorId,
            jobId,
            candidateId,
            command.Name,
            command.AppointmentDate
        );
        await _mediator.Send(commandWithId);
        return NoContent();
    }

    [HttpPost("{candidateId}/rejections")]
    public async Task<IActionResult> Rejected(string jobId, string candidateId)
    {
        var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var commandWithId = new RejectedCommand(
            authorId,
            jobId,
            candidateId
        );
        await _mediator.Send(commandWithId);
        return NoContent();
    }

    [HttpPost("{candidateId}/selection")]
    public async Task<IActionResult> Selected(string jobId, string candidateId)
    {
        var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var commandWithId = new SelectedCommand(
            authorId,
            jobId,
            candidateId
        );
        await _mediator.Send(commandWithId);
        return NoContent();
    }

}