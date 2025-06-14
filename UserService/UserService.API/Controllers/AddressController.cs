using JobTracker.UserService.Application.Addresses.Commands.CreateAddress;
using JobTracker.UserService.Application.Addresses.Commands.DeleteAddress;
using JobTracker.UserService.Application.Addresses.Commands.UpdateAddress;
using JobTracker.UserService.Application.Addresses.Queries.GetAddress;
using JobTracker.UserService.Application.Addresses.Queries.GetAddresses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediatR;

namespace JobTracker.UserService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/users/me/addresses")]
public class AddressController : ControllerBase
{
    private readonly IMediator _mediator;

    public AddressController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAddresses()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var addresses = await _mediator.Send(new GetAddressesQuery(userId));
        return Ok(addresses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAddressById(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var addresse = await _mediator.Send(new GetAddressQuery(userId, id));
        return Ok(addresse);
    }

    [HttpPost]
    public async Task<IActionResult> AddAddress([FromBody] CreateAddressCommand command)
    {
        if (command == null)
            return BadRequest("Invalid input data.");
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var commandWithId = new CreateAddressWithIdCommand(
            userId,
            command.Address1,
            command.Address2,
            command.PostalCode,
            command.City,
            command.County,
            command.State,
            command.Country
        );
        var addressId = await _mediator.Send(commandWithId);
        return CreatedAtAction(nameof(GetAddressById), new { id = addressId }, commandWithId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAddress(string id, [FromBody] UpdateAddressCommand command)
    {
        if (command == null)
            return BadRequest("Invalid input data.");
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        var commandWithId = new UpdateAddressWithIdCommand(
            userId,
            id,
            command.Address1,
            command.Address2,
            command.PostalCode,
            command.City,
            command.County,
            command.State,
            command.Country);
            
        await _mediator.Send(commandWithId);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAddress(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        await _mediator.Send(new DeleteAddressCommand(userId, id));
        return NoContent();
    }
}